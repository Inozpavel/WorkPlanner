using System;

namespace Users.Data
{
    public class ProfileUpdated
    {
        public Guid UserId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string PhoneNumber { get; set; }
    }
}