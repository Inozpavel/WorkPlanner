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
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "If user is unauthorized")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomsController(RoomService roomService) => _roomService = roomService;

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "If user hasn`t any rooms")]
        public async Task<ActionResult<IEnumerable<RoomViewModel>>> AllRoomsForUser()
        {
            var rooms = await _roomService.FindRoomsForUser(GetUserId());
            if (!rooms.Any())
                return NoContent();
            return Ok(rooms);
        }

        [HttpPut("{roomId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If id is incorrect", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "If user has insufficient rights", typeof(ProblemDetails))]
        public async Task<ActionResult> Update(RoomRequest request, Guid roomId)
        {
            await _roomService.UpdateRoom(request, roomId, GetUserId());
            return Ok();
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created)]
        public async Task<CreatedAtActionResult> Create(RoomRequest request)
        {
            var createdRoom = await _roomService.CreateRoom(request, GetUserId());
            return CreatedAtAction(nameof(Find), new {roomId = createdRoom.RoomId}, createdRoom);
        }

        [HttpDelete("{roomId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If id is incorrect", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "If user has insufficient rights", typeof(ProblemDetails))]
        public async Task<ActionResult> Delete(Guid roomId)
        {
            await _roomService.DeleteRoom(roomId, GetUserId());
            return Ok();
        }

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

        [HttpGet("/joinroom/{roomId:guid}")]
        [SwaggerResponse(StatusCodes.Status202Accepted)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If link is incorrect", typeof(ProblemDetails))]
        public async Task<ActionResult> JoinRoom(Guid roomId)
        {
            await _roomService.JoinUserToRoom(roomId, GetUserId());
            return Accepted();
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue("sub"));
    }
}