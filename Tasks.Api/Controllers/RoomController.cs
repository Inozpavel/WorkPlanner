using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasks.Api.DTOs;
using Tasks.Api.Services;

namespace Tasks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService) => _roomService = roomService;

        [Authorize]
        [HttpPost("rooms")]
        public async Task<ActionResult> Create(RoomRequest request)
        {
            await _roomService.CreateRoom(request, GetUserId());
            return Ok();
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}