using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tasks.Api.DTOs;
using Tasks.Api.Services;
using Tasks.Api.ViewModels;

namespace Tasks.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly RoomTaskService _roomTaskService;

        public TasksController(RoomTaskService roomTaskService) => _roomTaskService = roomTaskService;

        [HttpGet("{taskId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskViewModel>> Find(Guid taskId)
        {
            var task = await _roomTaskService.FindTask(taskId);
            if (task == null)
                return NotFound();
            return Ok(task);
        }


        [HttpGet("room/{roomId:guid}")]
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

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created)]
        public async Task<CreatedAtActionResult> Create(TaskRequest request)
        {
            var addedTask = await _roomTaskService.CreateTask(request, GetUserId());
            return CreatedAtAction(nameof(Find), new {taskId = addedTask.RoomTaskId}, addedTask);
        }

        [HttpPut("{taskId:guid}")]
        [SwaggerResponse(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> Update(Guid taskId, TaskRequest request)
        {
            await _roomTaskService.UpdateTask(taskId, request, GetUserId());
            return Accepted();
        }

        [HttpDelete("{taskId:guid}")]
        [SwaggerResponse(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> Delete(Guid taskId)
        {
            await _roomTaskService.DeleteTask(taskId, GetUserId());
            return Accepted();
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue("sub"));
    }
}