using System;
using System.Linq;
using Tasks.Api.Entities;

namespace Tasks.Api.Data
{
    public class RoomRepository : Repository<Room>
    {
        public RoomRepository(ApplicationContext context) : base(context)
        {
        }

        public Room? FindUserInRoomWithRole(Guid roomId, Guid userId, string roleName)
        {
            return _context.Rooms.FirstOrDefault(x =>
                x.RoomId == roomId &&
                x.UsersInRoom.Any(u => u.UserId == userId && u.RoomRole.RoomRoleName == roleName));
        }
    }
}