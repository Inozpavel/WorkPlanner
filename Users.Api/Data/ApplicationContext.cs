using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Users.Data.Entities;

namespace Users.Api.Data
{
    public class ApplicationContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
    }
}