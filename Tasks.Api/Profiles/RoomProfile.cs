using AutoMapper;
using Tasks.Api.Entities;
using Tasks.Api.ViewModels.RoomViewModels;

namespace Tasks.Api.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<AddRoomViewModel, Room>();

            CreateMap<Room, RoomViewModel>();
        }
    }
}