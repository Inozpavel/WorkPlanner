using System;

namespace Tasks.Api.ViewModels.RoleViewModels
{
    public class UpdateUserRoleViewModel
    {
        public Guid UserId { get; set; }

        public Guid RoomRoleId { get; set; }
    }
}