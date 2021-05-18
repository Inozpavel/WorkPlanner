using System;
using Tasks.Api.ViewModels.UserViewModel;

namespace Tasks.Api.ViewModels.RoleViewModels
{
    public class UserWithRoleViewModel
    {
        public UserFullNameViewModel User { get; set; }

        public Guid RoleId { get; set; }
    }
}