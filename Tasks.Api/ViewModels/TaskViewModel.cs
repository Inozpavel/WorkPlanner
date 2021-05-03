using System;

namespace Tasks.Api.ViewModels
{
    public class TaskViewModel
    {
        public Guid RoomTaskId { get; set; }

        public string TaskName { get; set; }

        public string TaskContent { get; set; }

        public string Details { get; set; }

        public DateTime TaskCreationTime { get; set; } = DateTime.Now;

        public Guid TaskCreatorId { get; set; }

        public DateTime DeadlineTime { get; set; }
    }
}