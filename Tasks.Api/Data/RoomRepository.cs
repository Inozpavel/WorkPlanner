using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tasks.Api.Entities;

namespace Tasks.Api.Data
{
    public class RoomRepository : Repository<Room>
    {
        public RoomRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<Room?> FindUserInRoomWithRole(Guid roomId, Guid userId, string roleName)
        {
            return await _context.Rooms.FirstOrDefaultAsync(x =>
                x.RoomId == roomId &&
                x.UsersInRoom.Any(u => u.UserId == userId && u.RoomRole.RoomRoleName == roleName));
        }

        public async Task<IEnumerable<Room>> FindRoomsForUser(Guid userId) =>
            await _context.Rooms.Where(x => x.UsersInRoom.Any(u => u.UserId == userId)).ToListAsync();

        public async Task<Room?> FindRoomWithUsers(Guid roomId) =>
            await _context.Rooms.Include(x => x.UsersInRoom).FirstOrDefaultAsync(x => x.RoomId == roomId);
    }
}