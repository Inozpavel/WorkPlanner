using AutoMapper;
using Tasks.Api.DTOs;
using Tasks.Api.Entities;

namespace Tasks.Api.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile() => CreateMap<RoomRequest, Room>();
    }
}