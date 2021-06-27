using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tasks.Api.Services;
using Tasks.Api.ViewModels.RoleViewModels;

namespace Tasks.Api.Controllers
{
    /// <summary>
    ///     Everything about roles
    /// </summary>
    [ApiController]
    [Route("api/")]
    [SwaggerTag("Everything about roles")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService) => _roleService = roleService;

        /// <summary>
        ///     Finds all roles in rooms
        /// </summary>
        /// <returns></returns>
        [HttpGet("[controller]")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<RoleViewModel>))]
        public async Task<ActionResult<List<string>>> AllRoles() => Ok(await _roleService.AllRoles());

        /// <summary>
        ///     Updates user role in room
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("rooms/{roomId:guid}/[controller]")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "If user is unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If id is incorrect", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "If user has insufficient rights", typeof(ProblemDetails))]
        public async Task<ActionResult> UpdateRoleForUser(Guid roomId, UpdateUserRoleViewModel viewModel)
        {
            await _roleService.UpdateRoleForUser(roomId, viewModel, UserService.GetCurrentUserId(HttpContext));
            return Ok();
        }

        /// <summary>
        ///     Returns information about user role in room
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("rooms/{roomId:guid}/[controller]")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<UserWithRoleViewModel>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "If user is unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If id is incorrect", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "If user has insufficient rights", typeof(ProblemDetails))]
        public async Task<ActionResult<List<UserWithRoleViewModel>>> UsersInRoomWIthRoles(Guid roomId)
        {
            var result = await _roleService.GetUsersInRoomWithRoles(roomId, UserService.GetCurrentUserId(HttpContext));
            return Ok(result);
        }
    }
}