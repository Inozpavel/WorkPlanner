using AutoMapper;
using Tasks.Api.DTOs;
using Tasks.Api.Entities;
using Tasks.Api.ViewModels;

namespace Tasks.Api.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<RoomRequest, Room>();

            CreateMap<Room, RoomViewModel>();
        }
    }
}