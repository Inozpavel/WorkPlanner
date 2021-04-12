using System;

namespace Tasks.Api.DTOs
{
    public class TaskRequest
    {
        public string TaskName { get; set; }

        public string TaskContent { get; set; }

        public string Details { get; set; }

        public DateTime DeadlineTime { get; set; }

        public Guid RoomId { get; set; }
    }
}