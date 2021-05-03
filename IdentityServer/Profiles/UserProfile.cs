using AutoMapper;
using IdentityServer.Entities;
using IdentityServer.ViewModels;

namespace IdentityServer.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.Email));
        }
    }
}