using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Users.Api.Exceptions;
using Users.Api.ViewModels;
using Users.Data.Entities;

namespace Users.Api.Services
{
    public class UserService
    {
        private readonly IConfiguration _configuration;

        private readonly EmailService _emailService;

        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        public UserService(IConfiguration configuration, UserManager<User> userManager, EmailService emailService,
            IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task Register(RegisterViewModel viewModel)
        {
            var user = _mapper.Map<User>(viewModel);
            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                await SendConfirmationMail(viewModel.Email);
                return;
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

            throw new RegistrationException(validationProblemDetails);
        }

        public async Task<bool> SendConfirmationMail(string email)
        {
            try
            {
                var createdUser = await _userManager.FindByEmailAsync(email);
                string token =
                    Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(createdUser));
                string callbackUrl =
                    $"{_configuration["Gateway:Host"]}/{_configuration["Gateway:ConfirmMailRoute"]}/{createdUser.Id}/{token}";

                await _emailService.SendEmailAsync(email, "Email confirmation",
                    string.Format(await File.ReadAllTextAsync("mail.html"), callbackUrl));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task UpdateProfileAsync(Guid userId, UpdateProfileViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new UserNotFoundApiException("Token is invalid: user with given id was not found!");

            user = _mapper.Map(viewModel, user);
            await _userManager.UpdateAsync(user);
        }

        public static Guid GetCurrentUserId(HttpContext context) =>
            Guid.Parse(context.User.FindFirstValue("sub") ?? string.Empty);

        private static void AddErrorsForKeyIfNotEmpty(ValidationProblemDetails details, string key, List<string> errors)
        {
            if (!errors.Any())
                return;
            details.Errors[key] = errors.ToArray();
        }
    }
}