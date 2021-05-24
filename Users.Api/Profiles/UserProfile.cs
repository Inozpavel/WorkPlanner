using AutoMapper;
using Users.Api.ViewModels;
using Users.Data;
using Users.Data.Entities;

namespace Users.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(dst => dst.UserName, options => options.MapFrom(src => src.Email));

            CreateMap<UpdateProfileViewModel, User>();

            CreateMap<User, UserRegistered>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<UpdateProfileViewModel, ProfileUpdated>();
        }
    }
}