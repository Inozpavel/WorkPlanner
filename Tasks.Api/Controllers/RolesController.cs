using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tasks.Api.DTOs;
using Tasks.Api.Services;

namespace Tasks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService) => _roleService = roleService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<string>>> AllRoles() => Ok(await _roleService.AllRoles());

        [Authorize]
        [HttpPut("room")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "If user is unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "If id or role name is incorrect", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "If user has insufficient rights", typeof(ProblemDetails))]
        public async Task<ActionResult> UpdateRoleForUser(UpdateUserRoleRequest request)
        {
            await _roleService.UpdateRoleForUser(request, GetUserId());
            return Ok();
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue("sub"));
    }
}