using System;
using System.Collections.Generic;

namespace Tasks.Api.Entities
{
    public class UserInRoom
    {
        public Guid RoomId { get; set; }

        public Guid UserId { get; set; }

        public List<RoomRole> RoomRoles { get; set; }
    }
}