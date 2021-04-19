using System;

namespace Tasks.Api.DTOs
{
    public class UpdateUserRoleRequest
    {
        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }

        public Guid RoleId { get; set; }
    }
}