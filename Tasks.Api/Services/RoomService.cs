using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Tasks.Api.Data;
using Tasks.Api.DTOs;
using Tasks.Api.Entities;
using Tasks.Api.Exceptions;
using Tasks.Api.ViewModel;

namespace Tasks.Api.Services
{
    public class RoomService
    {
        private readonly IMapper _mapper;

        private readonly UnitOfWork _unitOfWork;

        public RoomService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoomViewModel> CreateRoom(RoomRequest request, Guid userId)
        {
            var room = _mapper.Map<Room>(request);
            room.UsersInRoom = new List<UserInRoom>
            {
                new()
                {
                    UserId = userId,
                    RoomRole = await _unitOfWork.RoomRoleRepository.Find(x => x.RoomRoleName == Roles.Creator)
                }
            };

            var createdRoom = await _unitOfWork.RoomRepository.Create(room);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RoomViewModel>(createdRoom);
        }

        public async Task UpdateRoom(RoomRequest request, Guid roomId, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.Find(x => x.RoomId == roomId);

            if (room == null)
                throw new NotFoundException("Room with given id was not found!");

            if (!await CheckUserHasRole(roomId, userId, Roles.Creator) ||
                !await CheckUserHasRole(roomId, userId, Roles.Administrator))
                throw new AccessRightException("The user has insufficient rights");

            room = _mapper.Map(request, room);

            _unitOfWork.RoomRepository.Update(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRoom(Guid roomId, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.Find(x =>
                x.RoomId == roomId);

            if (room == null)
                throw new NotFoundException("Room with given id was not found!");

            if (!await CheckUserHasRole(roomId, userId, Roles.Creator))
                throw new AccessRightException("Insufficient access rights! Only owner can delete room!");

            _unitOfWork.RoomRepository.Delete(room);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<bool> CheckUserHasRole(Guid roomId, Guid userId, string role) =>
            await _unitOfWork.RoomRepository.FindUserInRoomWithRole(roomId, userId, role) != null;

        public async Task<IEnumerable<Room>> FindRoomsForUser(Guid userId) =>
            await _unitOfWork.RoomRepository.FindRoomsForUser(userId);

        public async Task<Room?> FindRoomWithUsers(Guid roomId) =>
            await _unitOfWork.RoomRepository.FindRoomWithUsers(roomId);

        public async Task<Room?> FindById(Guid roomId) =>
            await _unitOfWork.RoomRepository.Find(x => x.RoomId == roomId);

        public async Task JoinUserToRoom(Guid roomId, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.FindRoomWithUsers(roomId);

            if (room == null)
                throw new NotFoundException("Incorrect link!");

            if (room.UsersInRoom.Any(x => x.UserId == userId))
                return;

            room.UsersInRoom.Add(new UserInRoom
            {
                UserId = userId,
                RoomRole = await _unitOfWork.RoomRoleRepository.Find(x => x.RoomRoleName == Roles.Member)
            });
            await _unitOfWork.SaveChangesAsync();
        }
    }
}