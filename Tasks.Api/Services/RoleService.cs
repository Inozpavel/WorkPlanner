using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Tasks.Api.Data;
using Tasks.Api.DTOs;
using Tasks.Api.Entities;
using Tasks.Api.Exceptions;
using Tasks.Api.ViewModels;

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

        public async Task UpdateRoleForUser(UpdateUserRoleRequest request, Guid senderId)
        {
            if (await _roomService.FindById(request.RoomId) == null)
                throw new NotFoundApiException(AppExceptions.RoomNotFoundException);

            if (!await _userService.CheckUserIsInRoom(request.RoomId, senderId))
                throw new AccessRightApiException(AppExceptions.NotRoomMemberException);

            if (!await _userService.CheckUserIsInRoom(request.RoomId, request.UserId))
                throw new AccessRightApiException(AppExceptions.NotRoomMemberException);

            if (!await _userService.CheckUserIsInRole(request.RoomId, senderId, Roles.Creator))
                throw new AccessRightApiException(AppExceptions.CreatorOnlyCanPerformThisActionException);

            if (request.UserId == senderId)
                return;

            var role = await _unitOfWork.RoomRoleRepository.Find(x => x.RoomRoleId == request.RoleId);
            if (role == null)
                throw new NotFoundApiException(AppExceptions.RoleNotFoundException);

            var user = await _unitOfWork.RoomRepository.FindUserInRoom(request.RoomId, request.UserId);
            if (user != null)
                user.RoomRole = role;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}