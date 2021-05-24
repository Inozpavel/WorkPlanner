using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Tasks.Api.Data;
using Tasks.Api.Entities;
using Tasks.Api.Exceptions;
using Tasks.Api.ViewModels.RoleViewModels;
using Tasks.Api.ViewModels.UserViewModel;

namespace Tasks.Api.Services
{
    public class RoleService
    {
        private readonly IMapper _mapper;

        private readonly RoomService _roomService;

        private readonly UnitOfWork _unitOfWork;

        private readonly UserService _userService;

        public RoleService(UnitOfWork unitOfWork, UserService userService, RoomService roomService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roomService = roomService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleViewModel>> AllRoles()
        {
            var roles = await _unitOfWork.RoomRoleRepository.FindAllRoles();
            return _mapper.Map<IEnumerable<RoleViewModel>>(roles);
        }

        public async Task UpdateRoleForUser(Guid roomId, UpdateUserRoleViewModel viewModel, Guid userId)
        {
            await _roomService.ThrowIfRoomNotFound(roomId);
            await _userService.ThrowIfNotRoomMember(roomId, userId);

            if (!await _userService.CheckUserIsInRole(roomId, userId, Roles.Owner))
                throw new AccessRightApiException(AppExceptions.CreatorOnlyCanPerformThisActionException);

            if (viewModel.UserId == userId)
                return;

            var role = await _unitOfWork.RoomRoleRepository.Find(x => x.RoomRoleId == viewModel.RoomRoleId);
            if (role == null)
                throw new NotFoundApiException(AppExceptions.RoleNotFoundException);

            var user = await _unitOfWork.RoomRepository.FindUserInRoom(roomId, viewModel.UserId);
            if (user != null)
                user.RoomRole = role;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<UserWithRoleViewModel>> GetUsersInRoomWithRoles(Guid roomId, Guid userId)
        {
            await _roomService.ThrowIfRoomNotFound(roomId);
            await _userService.ThrowIfNotRoomMember(roomId, userId);

            var room = await _unitOfWork.RoomRepository.FindRoomWithUsers(roomId);
            if (room == null)
                throw new NotFoundApiException(AppExceptions.RoomNotFoundException);

            return room.UsersInRoom.Select(x => new UserWithRoleViewModel
            {
                User = _mapper.Map<UserFullNameViewModel>(x.User),
                RoleId = x.RoomRoleId
            }).ToList();
        }
    }
}