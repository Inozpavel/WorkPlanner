using AutoMapper;
using Tasks.Api.Entities;
using Tasks.Api.ViewModels.RoleViewModels;

namespace Tasks.Api.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoomRole, RoleViewModel>();
        }
    }
}