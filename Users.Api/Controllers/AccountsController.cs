using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Users.Api.Services;
using Users.Api.ViewModels;
using Users.Data.Entities;

namespace Users.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        
        private readonly UserService _userService;

        public AccountsController(UserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPost("[action]")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If data is invalid", typeof(ValidationProblemDetails))]
        public async Task<ActionResult> RegisterAsync(RegisterViewModel viewModel)
        {
            await _userService.Register(viewModel);
            return Ok();
        }

        [HttpGet("confirm-email/{userId}/{token}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ConfirmEmailAsync(string userId, string token)
        {
            if (!Guid.TryParse(userId, out _))
                return BadRequest("User id is not guid");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User id is invalid");

            var result = await _userManager.ConfirmEmailAsync(user, Uri.UnescapeDataString(token));
            if (!result.Succeeded)
                return BadRequest("Token is invalid");

            return Ok("Ok");
        }

        [HttpGet("resend-confirm-mail")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If email if not registered", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "If email if already confirmed", typeof(ProblemDetails))]
        public async Task<ActionResult> ReSendEmailAsync([FromHeader] string registeredEmail)
        {
            var user = await _userManager.FindByEmailAsync(registeredEmail);
            if (user == null)
                return BadRequest(new ProblemDetails
                {
                    Detail = "Email is not registered!"
                });
            if (user.EmailConfirmed)
                return Conflict(new ProblemDetails
                {
                    Detail = "Already confirmed"
                });

            await _userService.SendConfirmationMail(registeredEmail);
            return Ok();
        }

        [Authorize]
        [HttpPost("[action]")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If token is invalid", typeof(ProblemDetails))]
        public async Task<ActionResult> Profile(UpdateProfileViewModel viewModel)
        {
            await _userService.UpdateProfileAsync(UserService.GetCurrentUserId(HttpContext), viewModel);
            return Ok();
        }
    }
}