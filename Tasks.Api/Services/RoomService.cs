using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Tasks.Api.Data;
using Tasks.Api.DTOs;
using Tasks.Api.Entities;
using Tasks.Api.Exceptions;

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
            room.UsersInRoom = new List<UserInRoom>
            {
                new()
                {
                    UserId = userId,
                    RoomRole = await _unitOfWork.RoomRoleRepository.Find(x => x.RoomRoleName == Roles.Creator)
                }
            };

            await _unitOfWork.RoomRepository.Create(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateRoom(RoomRequest request, Guid roomId, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.Find(x => x.RoomId == roomId);

            if (room == null)
                throw new NotFoundException("Room with given id was not found!");

            if (!CheckUserHasRole(roomId, userId, Roles.Creator) ||
                !CheckUserHasRole(roomId, userId, Roles.Administrator))
                throw new AccessRightException("The user has insufficient rights");

            room = _mapper.Map(request, room);

            _unitOfWork.RoomRepository.Update(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRoom(Guid roomId, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.Find(x =>
                x.RoomId == roomId);

            if (room == null)
                throw new NotFoundException("Room with given id was not found!");

            if (!CheckUserHasRole(roomId, userId, Roles.Creator))
                throw new AccessRightException("Insufficient access rights! Only owner can delete room!");

            _unitOfWork.RoomRepository.Delete(room);
            await _unitOfWork.SaveChangesAsync();
        }

        private bool CheckUserHasRole(Guid roomId, Guid userId, string role) =>
            _unitOfWork.RoomRepository.FindUserInRoomWithRole(roomId, userId, role) != null;
    }
}