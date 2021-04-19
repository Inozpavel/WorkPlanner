using System;

namespace Tasks.Api.ViewModels
{
    public class RoleViewModel
    {
        public Guid RoomRoleId { get; set; }

        public string RoomRoleName { get; set; }

        public string? RoomRoleDescription { get; set; }
    }
}