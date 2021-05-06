using System.ComponentModel.DataAnnotations;

namespace Users.Api.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string Patronymic { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}