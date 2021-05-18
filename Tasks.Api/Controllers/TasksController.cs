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
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskViewModel>> Find(Guid roomId, Guid taskId)
        {
            var task = await _roomTaskService.FindTask(roomId, taskId);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created)]
        public async Task<CreatedAtActionResult> Create(Guid roomId, AddTaskViewModel viewModel)
        {
            var addedTask =
                await _roomTaskService.CreateTask(roomId, viewModel, UserService.GetCurrentUserId(HttpContext));
            return CreatedAtAction(nameof(Find), new {roomId = addedTask.RoomTaskId, taskId = addedTask.RoomTaskId},
                addedTask);
        }

        [HttpPut("{taskId:guid}")]
        [SwaggerResponse(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> Update(Guid roomId, Guid taskId, AddTaskViewModel viewModel)
        {
            await _roomTaskService.UpdateTask(roomId, taskId, viewModel, UserService.GetCurrentUserId(HttpContext));
            return Accepted();
        }

        [HttpDelete("{taskId:guid}")]
        [SwaggerResponse(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> Delete(Guid roomId, Guid taskId)
        {
            await _roomTaskService.DeleteTask(roomId, taskId, UserService.GetCurrentUserId(HttpContext));
            return Accepted();
        }
    }
}