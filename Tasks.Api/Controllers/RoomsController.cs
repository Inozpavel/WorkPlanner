using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tasks.Api.Services;
using Tasks.Api.ViewModels.RoomViewModels;

namespace Tasks.Api.Controllers
{
    /// <summary>
    ///     Everything about rooms
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "If user is unauthorized")]
    [SwaggerTag("Everything about rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomService _roomService;

        /// <inheritdoc />
        public RoomsController(RoomService roomService) => _roomService = roomService;

        /// <summary>
        ///     Finds all rooms for user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "If user hasn`t any rooms")]
        public async Task<ActionResult<IEnumerable<RoomViewModel>>> AllRoomsForUser()
        {
            var rooms = await _roomService.FindRoomsForUser(UserService.GetCurrentUserId(HttpContext));
            if (!rooms.Any())
                return NoContent();
            return Ok(rooms);
        }

        /// <summary>
        ///     Updates room information by id
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpPut("{roomId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If id is incorrect", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "If user has insufficient rights", typeof(ProblemDetails))]
        public async Task<ActionResult> Update(AddOrUpdateRoomViewModel viewModel, Guid roomId)
        {
            await _roomService.UpdateRoom(viewModel, roomId, UserService.GetCurrentUserId(HttpContext));
            return Ok();
        }

        /// <summary>
        ///     Creates new room for user
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(RoomViewModel))]
        public async Task<CreatedAtActionResult> Create(AddOrUpdateRoomViewModel viewModel)
        {
            var createdRoom = await _roomService.CreateRoom(viewModel, UserService.GetCurrentUserId(HttpContext));
            return CreatedAtAction(nameof(Find), new {roomId = createdRoom.RoomId}, createdRoom);
        }

        /// <summary>
        ///     Deletes room by id
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpDelete("{roomId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If id is incorrect", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "If user has insufficient rights", typeof(ProblemDetails))]
        public async Task<ActionResult> Delete(Guid roomId)
        {
            await _roomService.DeleteRoom(roomId, UserService.GetCurrentUserId(HttpContext));
            return Ok();
        }

        /// <summary>
        ///     Finds room by id
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpGet("{roomId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If id is incorrect", typeof(ProblemDetails))]
        public async Task<ActionResult<RoomViewModel>> Find(Guid roomId)
        {
            var room = await _roomService.FindById(roomId);
            if (room == null)
                return NotFound();
            return Ok(room);
        }

        /// <summary>
        ///     Joins user to new room by id
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpGet("join/{roomId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(RoomViewModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If link is incorrect", typeof(ProblemDetails))]
        public async Task<ActionResult<RoomViewModel>> JoinRoom(Guid roomId)
        {
            var room = await _roomService.JoinUserToRoom(roomId, UserService.GetCurrentUserId(HttpContext));
            return Ok(room);
        }
    }
}