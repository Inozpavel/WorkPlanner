using AutoMapper;
using Tasks.Api.DTOs;
using Tasks.Api.Entities;
using Tasks.Api.ViewModels;

namespace Tasks.Api.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskRequest, RoomTask>()
                .ForMember(dest => dest.RoomId, cfg => cfg.Ignore());

            CreateMap<RoomTask, TaskViewModel>();
        }
    }
}