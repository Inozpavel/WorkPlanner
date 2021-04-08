using System;
using System.Collections.Generic;

namespace Tasks.Api.Entities
{
    public class RoomRole
    {
        public Guid RoomRoleId { get; set; }

        public string RoomRoleName { get; set; }

        public string? RoomRoleDescription { get; set; }

        public List<UserInRoom> UserInRooms { get; set; }
    }
}