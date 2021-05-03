using AutoMapper;
using Tasks.Api.Entities;
using Tasks.Api.ViewModels;

namespace Tasks.Api.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<AddTaskViewModel, RoomTask>()
                .ForMember(dest => dest.RoomId, cfg => cfg.Ignore());

            CreateMap<RoomTask, TaskViewModel>();
        }
    }
}