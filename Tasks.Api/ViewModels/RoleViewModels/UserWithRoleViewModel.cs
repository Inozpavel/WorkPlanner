using System;

namespace Tasks.Api.ViewModels.RoleViewModels
{
    public class UserWithRoleViewModel
    {
        public Guid UserId { get; set; }

        public Guid RoomRoleId { get; set; }
    }
}