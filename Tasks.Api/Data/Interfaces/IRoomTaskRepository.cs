using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasks.Api.Entities;

namespace Tasks.Api.Data.Interfaces
{
    public interface IRoomTaskRepository : IRepository<RoomTask>
    {
        Task<IEnumerable<RoomTask>> FindTasksInRoom(Guid roomId);
    }
}