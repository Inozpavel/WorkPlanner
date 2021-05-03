using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Tasks.Api.Data;
using Tasks.Api.Entities;
using Tasks.Api.Exceptions;
using Tasks.Api.ViewModels;

namespace Tasks.Api.Services
{
    public class RoomService
    {
        private readonly IMapper _mapper;

        private readonly UnitOfWork _unitOfWork;

        private readonly UserService _userService;

        public RoomService(IMapper mapper, UnitOfWork unitOfWork, UserService userService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<RoomViewModel> CreateRoom(AddRoomViewModel viewModel, Guid userId)
        {
            var room = _mapper.Map<Room>(viewModel);
            room.UsersInRoom = new List<UserInRoom>
            {
                new()
                {
                    UserId = userId,
                    RoomRole = await _unitOfWork.RoomRoleRepository.FindWithName(Roles.Creator)
                }
            };

            var createdRoom = await _unitOfWork.RoomRepository.Create(room);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RoomViewModel>(createdRoom);
        }

        public async Task UpdateRoom(AddRoomViewModel viewModel, Guid roomId, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.Find(x => x.RoomId == roomId);

            if (room == null)
                throw new NotFoundApiException(AppExceptions.RoomNotFoundException);

            if (!await _userService.CheckUserHasAnyRole(roomId, userId, Roles.Creator, Roles.Administrator))
                throw new AccessRightApiException(AppExceptions.CreatorOrAdministratorOnlyCanDoThisException);

            room = _mapper.Map(viewModel, room);

            _unitOfWork.RoomRepository.Update(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRoom(Guid roomId, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.Find(x => x.RoomId == roomId);

            if (room == null)
                throw new NotFoundApiException(AppExceptions.RoomNotFoundException);

            if (!await _userService.CheckUserIsInRole(roomId, userId, Roles.Creator))
                throw new AccessRightApiException(AppExceptions.CreatorOnlyCanPerformThisActionException);

            _unitOfWork.RoomRepository.Delete(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomViewModel>> FindRoomsForUser(Guid userId)
        {
            var rooms = await _unitOfWork.RoomRepository.FindRoomsForUser(userId);
            return _mapper.Map<IEnumerable<RoomViewModel>>(rooms);
        }

        public async Task<Room?> FindRoomWithUsers(Guid roomId) =>
            await _unitOfWork.RoomRepository.FindRoomWithUsers(roomId);

        public async Task<RoomViewModel?> FindById(Guid roomId)
        {
            var room = await _unitOfWork.RoomRepository.Find(x => x.RoomId == roomId);
            return _mapper.Map<RoomViewModel>(room);
        }

        public async Task JoinUserToRoom(Guid roomId, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.FindRoomWithUsers(roomId);

            if (room == null)
                throw new NotFoundApiException(AppExceptions.IncorrectUrlException);

            if (room.UsersInRoom.Any(x => x.UserId == userId))
                return;

            room.UsersInRoom.Add(new UserInRoom
            {
                UserId = userId,
                RoomRole = await _unitOfWork.RoomRoleRepository.FindWithName(Roles.Member)
            });
            await _unitOfWork.SaveChangesAsync();
        }
    }
}