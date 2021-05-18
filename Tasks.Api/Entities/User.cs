using System;
using System.Collections.Generic;

namespace Tasks.Api.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public List<UserInTheRoom> UserInTheRooms { get; set; }
    }
}