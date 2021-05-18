using AutoMapper;
using Tasks.Api.Entities;
using Tasks.Api.ViewModels.RoleViewModels;

namespace Tasks.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInRoom, UserWithRoleViewModel>();
        }
    }
}