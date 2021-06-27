using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tasks.Api.Data.Interfaces;
using Tasks.Api.Entities;

namespace Tasks.Api.Data.Repositories
{
    public class RoomTaskRepository : Repository<RoomTask>, IRoomTaskRepository
    {
        public RoomTaskRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RoomTask>> FindTasksInRoom(Guid roomId)
        {
            var room = await _context.Rooms.Include(x => x.RoomTasks).FirstOrDefaultAsync(r => r.RoomId == roomId);
            return room?.RoomTasks ?? new List<RoomTask>();
        }
    }
}