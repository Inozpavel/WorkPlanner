#nullable enable
using System;
using Microsoft.AspNetCore.Identity;

namespace Users.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string? Patronymic { get; set; }
    }
}