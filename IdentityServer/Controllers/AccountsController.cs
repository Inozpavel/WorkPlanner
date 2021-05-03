using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer.Entities;
using IdentityServer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly EmailService _emailService;

        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        public AccountsController(IConfiguration configuration, EmailService emailService, IMapper mapper,
            UserManager<User> userManager)
        {
            _configuration = configuration;
            _emailService = emailService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If data is invalid", typeof(ValidationProblemDetails))]
        public async Task<ActionResult> RegisterAsync(RegisterViewModel viewModel)
        {
            var user = _mapper.Map<User>(viewModel);
            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                await SendConfirmationMail(viewModel.Email);
                return Ok();
            }

            var validationProblemDetails = new ValidationProblemDetails();
            List<string> passwordErrors = new();
            List<string> emailErrors = new();
            List<string> otherErrors = new();

            result.Errors.Select(x => x.Description).ToList().ForEach(x =>
            {
                if (x.Contains("password", StringComparison.InvariantCultureIgnoreCase))
                    passwordErrors.Add(x);
                else if (x.Contains("email", StringComparison.InvariantCultureIgnoreCase))
                    emailErrors.Add(x);
                else
                    otherErrors.Add(x);
            });

            AddErrorsForKeyIfNotEmpty(validationProblemDetails, "Email", emailErrors);
            AddErrorsForKeyIfNotEmpty(validationProblemDetails, "Password", passwordErrors);
            AddErrorsForKeyIfNotEmpty(validationProblemDetails, "Other", otherErrors);

            return BadRequest(validationProblemDetails);
        }

        [HttpGet("/confirm-email/{userId}/{token}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
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

        private static void AddErrorsForKeyIfNotEmpty(ValidationProblemDetails details, string key, List<string> errors)
        {
            if (!errors.Any())
                return;
            details.Errors[key] = errors.ToArray();
        }

        [HttpGet("/resend-confirm-mail")]
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

            await SendConfirmationMail(registeredEmail);
            return Ok();
        }

        private async Task<bool> SendConfirmationMail(string email)
        {
            try
            {
                var createdUser = await _userManager.FindByEmailAsync(email);
                string token =
                    Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(createdUser));
                string callbackUrl =
                    $"{_configuration["Gateway:Origin"]}/gateway/identity/confirm-email/{createdUser.Id}/{token}";

                await _emailService.SendEmailAsync(email, "Email confirmation",
                    string.Format(await System.IO.File.ReadAllTextAsync("mail.html"), callbackUrl));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}