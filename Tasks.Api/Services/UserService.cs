using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Tasks.Api.Data;

namespace Tasks.Api.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserService(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> CheckUserHasAnyRole(Guid roomId, Guid userId, params string[] rolesNames)
        {
            foreach (string roleName in rolesNames)
            {
                if (await CheckUserIsInRole(roomId, userId, roleName))
                    return true;
            }

            return false;
        }

        public static Guid GetCurrentUserId(HttpContext context) =>
            Guid.Parse(context.User.FindFirstValue("sub") ?? string.Empty);

        public async Task<bool> CheckUserIsInRole(Guid roomId, Guid userId, string roleName) =>
            await _unitOfWork.RoomRepository.FindRoomWithUserInRole(roomId, userId, roleName) != null;

        public async Task<bool> CheckUserIsInRoom(Guid roomId, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.FindRoomWithUsers(roomId);
            return room?.UsersInRoom.Any(x => x.UserId == userId) ?? false;
        }
    }
}