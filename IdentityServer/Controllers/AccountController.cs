using System.Linq;
using System.Threading.Tasks;
using IdentityServer.DTOs;
using IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("connect")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager) => _userManager = userManager;

        [HttpPost("[action]")]
        public async Task<ActionResult> RegisterAsync(RegisterUserRequest request)
        {
            var result = await _userManager.CreateAsync(new User
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Patronymic = request.Patronymic,
                PhoneNumber = request.PhoneNumber
            }, request.Password);

            if (result.Succeeded)
                return Ok("Registered");

            return BadRequest(result.Errors.Select(x => new ProblemDetails
            {
                Detail = x.Description
            }).ToList());
        }
    }
}