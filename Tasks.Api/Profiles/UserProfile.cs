using AutoMapper;
using Tasks.Api.Entities;
using Tasks.Api.ViewModels.RoleViewModels;
using Tasks.Api.ViewModels.UserViewModel;
using Users.Data;

namespace Tasks.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInTheRoom, UpdateUserRoleViewModel>();

            CreateMap<UserRegistered, User>();

            CreateMap<User, UserFullNameViewModel>();

            CreateMap<ProfileUpdated, User>();
        }
    }
}