using System.ComponentModel.DataAnnotations;

namespace IdentityServer.DTOs
{
    public class RegisterUserRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}