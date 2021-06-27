using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasks.Api.Entities;

namespace Tasks.Api.Data.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<UserInTheRoom?> FindUserInRoom(Guid roomId, Guid userId);

        Task<Room?> FindRoomWithUserInRole(Guid roomId, Guid userId, string roleName);

        Task<IEnumerable<Room>> FindRoomsForUser(Guid userId);

        Task<Room?> FindRoomWithUsers(Guid roomId);
    }
}