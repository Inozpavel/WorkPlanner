using System;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }
    }
}