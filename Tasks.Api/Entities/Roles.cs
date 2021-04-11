using System.Collections.Generic;

namespace Tasks.Api.Entities
{
    public class Roles
    {
        public const string Creator = "Creator";

        public const string Administrator = "Admin";

        public const string Member = "Member";

        public static IEnumerable<string> AllRoles()
        {
            yield return Creator;
            yield return Administrator;
            yield return Member;
        }
    }
}