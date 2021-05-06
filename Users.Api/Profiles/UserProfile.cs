using AutoMapper;
using Users.Api.ViewModels;
using Users.Data.Entities;

namespace Users.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(dst => dst.UserName, options => options.MapFrom(src => src.Email));
        }
    }
}