using AutoMapper;
using Tasks.Api.Entities;
using Tasks.Api.ViewModels.RoomViewModels;

namespace Tasks.Api.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<AddOrUpdateRoomViewModel, Room>();

            CreateMap<Room, RoomViewModel>();
        }
    }
}