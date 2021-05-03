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
using Swashbuckle.AspNetCore.Annotations;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        public AccountsController(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("[action]")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If data is invalid", typeof(ValidationProblemDetails))]
        public async Task<ActionResult> RegisterAsync(RegisterViewModel viewModel)
        {
            var user = _mapper.Map<User>(viewModel);
            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
                return Ok();

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

        private static void AddErrorsForKeyIfNotEmpty(ValidationProblemDetails details, string key, List<string> errors)
        {
            if (!errors.Any())
                return;
            details.Errors[key] = errors.ToArray();
        }
    }
}