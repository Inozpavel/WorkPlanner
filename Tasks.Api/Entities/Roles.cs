using System.Collections.Generic;

namespace Tasks.Api.Entities
{
    public class Roles
    {
        public const string Owner = "Owner";

        public const string Administrator = "Admin";

        public const string Member = "Member";

        public static IEnumerable<string> AllRoles()
        {
            yield return Owner;
            yield return Administrator;
            yield return Member;
        }
    }
}