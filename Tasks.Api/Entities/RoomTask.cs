using System;

namespace Tasks.Api.Entities
{
    public class RoomTask
    {
        public Guid RoomTaskId { get; set; }

        public string TaskName { get; set; }

        public string TaskContent { get; set; }

        public string? Details { get; set; }

        public bool IsCompleted { get; set; }

        public Guid RoomId { get; set; }

        public Room Room { get; set; }

        public DateTime TaskCreationTime { get; set; } = DateTime.Now;

        public Guid TaskCreatorId { get; set; }

        public User TaskCreator { get; set; }

        public DateTime DeadlineTime { get; set; }
    }
}