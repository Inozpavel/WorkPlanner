using System;

namespace Tasks.Api.ViewModels.UserViewModel
{
    public class UserFullNameViewModel
    {
        public Guid UserId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }
    }
}