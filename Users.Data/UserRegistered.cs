using System;

namespace Users.Data
{
    public class UserRegistered
    {
        public Guid UserId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }
    }
}