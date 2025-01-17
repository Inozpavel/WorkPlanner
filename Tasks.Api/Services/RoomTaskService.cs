﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Tasks.Api.Data;
using Tasks.Api.Entities;
using Tasks.Api.Exceptions;
using Tasks.Api.ViewModels.TaskViewModels;
using Tasks.Api.ViewModels.UserViewModel;

namespace Tasks.Api.Services
{
    public class RoomTaskService
    {
        private readonly IMapper _mapper;

        private readonly UnitOfWork _unitOfWork;

        private readonly UserService _userService;

        public RoomTaskService(IMapper mapper, UnitOfWork unitOfWork, UserService userService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<IEnumerable<TaskViewModel>> FindAllTasksInRoom(Guid roomId)
        {
            var tasks = await _unitOfWork.RoomTaskRepository.FindTasksInRoom(roomId);
            return _mapper.Map<IEnumerable<TaskViewModel>>(tasks);
        }

        public async Task<TaskViewModel?> FindTask(Guid roomId, Guid taskId, Guid userId)
        {
            var task = await _unitOfWork.RoomTaskRepository.Find(x => x.RoomTaskId == taskId);
            if (task?.RoomId != roomId)
                throw new NotFoundApiException(AppExceptions.TaskInRoomNotFoundException);

            await _userService.ThrowIfNotRoomMember(roomId, userId);

            return _mapper.Map<TaskViewModel>(task);
        }

        public async Task<TaskViewModel?> CompleteTask(Guid roomId, Guid taskId, Guid userId)
        {
            var task = await _unitOfWork.RoomTaskRepository.Find(x => x.RoomTaskId == taskId);
            if (task?.RoomId != roomId)
                throw new NotFoundApiException(AppExceptions.TaskInRoomNotFoundException);
            await _userService.ThrowIfNotRoomMember(roomId, userId);

            task.IsCompleted = true;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TaskViewModel>(task);
        }

        public async Task<TaskViewModel> CreateTask(Guid roomId, AddTaskViewModel viewModel, Guid userId)
        {
            var room = await _unitOfWork.RoomRepository.Find(r => r.RoomId == roomId);

            if (room == null)
                throw new NotFoundApiException(AppExceptions.RoomNotFoundException);

            await _userService.ThrowIfNotRoomMember(roomId, userId);

            if (!await _userService.CheckUserHasAnyRole(roomId, userId, Roles.Owner, Roles.Administrator))
                throw new AccessRightApiException(AppExceptions.CreatorOrAdministratorOnlyCanDoThisException);

            var task = _mapper.Map<RoomTask>(viewModel);
            task.TaskCreatorId = userId;
            task.Room = room;

            var addedTask = await _unitOfWork.RoomTaskRepository.Create(task);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaskViewModel>(addedTask);
        }

        public async Task UpdateTask(Guid roomId, Guid taskId, AddTaskViewModel viewModel, Guid userId)
        {
            var task = await _unitOfWork.RoomTaskRepository.Find(x => x.RoomTaskId == taskId);

            if (task == null)
                throw new NotFoundApiException(AppExceptions.TaskNotFoundException);

            ThrowIfTaskNotInRoom(task, roomId);
            await _userService.ThrowIfNotRoomMember(roomId, userId);

            if (!await _userService.CheckUserHasAnyRole(task.RoomId, userId, Roles.Owner, Roles.Administrator))
                throw new AccessRightApiException(AppExceptions.CreatorOrAdministratorOnlyCanDoThisException);

            task = _mapper.Map(viewModel, task);
            _unitOfWork.RoomTaskRepository.Update(task);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTask(Guid roomId, Guid taskId, Guid userId)
        {
            var task = await _unitOfWork.RoomTaskRepository.Find(x => x.RoomTaskId == taskId);

            if (task == null)
                throw new NotFoundApiException(AppExceptions.TaskNotFoundException);

            ThrowIfTaskNotInRoom(task, roomId);
            await _userService.ThrowIfNotRoomMember(roomId, userId);

            if (!await _userService.CheckUserHasAnyRole(task.RoomId, userId, Roles.Owner, Roles.Administrator))
                throw new AccessRightApiException(AppExceptions.CreatorOrAdministratorOnlyCanDoThisException);

            _unitOfWork.RoomTaskRepository.Delete(task);

            await _unitOfWork.SaveChangesAsync();
        }

        private static void ThrowIfTaskNotInRoom(RoomTask roomTask, Guid roomId)
        {
            if (roomTask.RoomId != roomId)
                throw new NotFoundApiException(AppExceptions.TaskInRoomNotFoundException);
        }

        public async Task<UserFullNameViewModel> FindTaskCreator(Guid roomId, Guid taskId, Guid userId)
        {
            var task = await _unitOfWork.RoomTaskRepository.Find(x => x.RoomTaskId == taskId);

            if (task == null)
                throw new NotFoundApiException(AppExceptions.TaskNotFoundException);

            ThrowIfTaskNotInRoom(task, roomId);
            await _userService.ThrowIfNotRoomMember(roomId, userId);

            var userInRoom = await _unitOfWork.RoomRepository.FindUserInRoom(roomId, task.TaskCreatorId);
            if (userInRoom == null)
            {
                throw new NotFoundApiException(AppExceptions.CreatorNotFound);
            }

            return _mapper.Map<UserFullNameViewModel>(userInRoom.User);
        }
    }
}