using AutoMapper;
using Tasks.Api.Entities;
using Tasks.Api.ViewModels.RoleViewModels;
using Users.Data;

namespace Tasks.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInTheRoom, UserWithRoleViewModel>();

            CreateMap<UserRegistered, User>();
        }
    }
}