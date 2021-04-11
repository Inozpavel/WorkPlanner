using System;

namespace Tasks.Api.Entities
{
    public class UserInRoom
    {
        public Guid RoomId { get; set; }

        public Guid UserId { get; set; }

        public RoomRole RoomRole { get; set; }

        public Guid RoomRoleId { get; set; }
    }
}