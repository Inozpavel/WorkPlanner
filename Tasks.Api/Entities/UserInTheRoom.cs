using System;

namespace Tasks.Api.Entities
{
    public class UserInTheRoom
    {
        public Guid RoomId { get; set; }

        public Room Room { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid RoomRoleId { get; set; }

        public RoomRole RoomRole { get; set; }
    }
}