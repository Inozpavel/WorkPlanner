using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tasks.Api.Services;
using Tasks.Api.ViewModels.TaskViewModels;
using Tasks.Api.ViewModels.UserViewModel;

namespace Tasks.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/room/{roomId:guid}/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly RoomTaskService _roomTaskService;

        public TasksController(RoomTaskService roomTaskService) => _roomTaskService = roomTaskService;

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> AllTasksInRoom(Guid roomId)
        {
            var tasks = await _roomTaskService.FindAllTasksInRoom(roomId);
            if (!tasks.Any())
                return NoContent();
            return Ok(tasks);
        }

        [HttpGet("{taskId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskViewModel>> Find(Guid roomId, Guid taskId)
        {
            var task = await _roomTaskService.FindTask(roomId, taskId, UserService.GetCurrentUserId(HttpContext));
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpGet("{taskId:guid}/creator")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserFullNameViewModel>> FindCreator(Guid roomId, Guid taskId)
        {
            var creator =
                await _roomTaskService.FindTaskCreator(roomId, taskId, UserService.GetCurrentUserId(HttpContext));
            return Ok(creator);
        }

        [HttpGet("{taskId:guid}/complete")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserFullNameViewModel>> CompleteTask(Guid roomId, Guid taskId) => Ok(
            await _roomTaskService.CompleteTask(roomId, taskId, UserService.GetCurrentUserId(HttpContext)));


        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<CreatedAtActionResult> Create(Guid roomId, AddTaskViewModel viewModel)
        {
            var addedTask =
                await _roomTaskService.CreateTask(roomId, viewModel, UserService.GetCurrentUserId(HttpContext));
            return CreatedAtAction(nameof(Find), new {roomId = addedTask.RoomTaskId, taskId = addedTask.RoomTaskId},
                addedTask);
        }

        [HttpPut("{taskId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid roomId, Guid taskId, AddTaskViewModel viewModel)
        {
            await _roomTaskService.UpdateTask(roomId, taskId, viewModel, UserService.GetCurrentUserId(HttpContext));
            return Ok();
        }

        [HttpDelete("{taskId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid roomId, Guid taskId)
        {
            await _roomTaskService.DeleteTask(roomId, taskId, UserService.GetCurrentUserId(HttpContext));
            return Ok();
        }
    }
}