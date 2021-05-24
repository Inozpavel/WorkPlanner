using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Users.Api.Exceptions;
using Users.Api.ViewModels;
using Users.Data;
using Users.Data.Entities;

namespace Users.Api.Services
{
    public class UserService
    {
        private readonly IConfiguration _configuration;

        private readonly EmailService _emailService;

        private readonly IWebHostEnvironment _environment;

        private readonly IMapper _mapper;

        private readonly IPublishEndpoint _publishEndpoint;

        private readonly UserManager<User> _userManager;

        public UserService(IWebHostEnvironment environment, IPublishEndpoint publishEndpoint,
            IConfiguration configuration, UserManager<User> userManager, EmailService emailService, IMapper mapper)
        {
            _environment = environment;
            _publishEndpoint = publishEndpoint;
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
                await _publishEndpoint.Publish(_mapper.Map<UserRegistered>(user));
                try
                {
                    await SendConfirmationMail(viewModel.Email);
                }
                catch (Exception)
                {
                    // ignored
                }

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

        public async Task SendConfirmationMail(string email)
        {
            var createdUser = await _userManager.FindByEmailAsync(email);
            string token =
                Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(createdUser));
            string callbackUrl =
                $"{_configuration["Gateway:Host"]}/{_configuration["Gateway:ConfirmMailRoute"]}/{createdUser.Id}/{token}";

            await _emailService.SendEmailAsync(email, "Email confirmation",
                string.Format(await File.ReadAllTextAsync(Path.Combine(_environment.ContentRootPath, "mail.html")),
                    callbackUrl));
        }

        public async Task UpdateProfileAsync(Guid userId, UpdateProfileViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new UserNotFoundApiException("Token is invalid: user with given id was not found!");

            var updatedProfile = _mapper.Map<ProfileUpdated>(viewModel);
            updatedProfile.UserId = user.Id;
            await _publishEndpoint.Publish(updatedProfile);

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