using System;
using System.Threading.Tasks;
using AutoMapper;
using Tasks.Api.Data;
using Tasks.Api.DTOs;
using Tasks.Api.Entities;

namespace Tasks.Api.Services
{
    public class RoomService
    {
        private readonly IMapper _mapper;

        private readonly UnitOfWork _unitOfWork;

        public RoomService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateRoom(RoomRequest request, Guid userId)
        {
            var room = _mapper.Map<Room>(request);
            room.OwnerId = userId;

            await _unitOfWork.RoomRepository.Create(room);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}