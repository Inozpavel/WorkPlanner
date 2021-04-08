using System;
using System.Collections.Generic;

namespace Tasks.Api.Entities
{
    public class Room
    {
        public Guid RoomId { get; set; }

        public Guid OwnerId { get; set; }

        public string RoomName { get; set; }

        public string? RoomDescription { get; set; }

        public DateTime CreationDate { get; set; }

        public List<UserInRoom> UsersInRoom { get; set; }
    }
}