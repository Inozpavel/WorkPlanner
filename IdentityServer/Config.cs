using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new()
                {
                    ClientId = "MobileApp",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        "TaskAPI"
                    }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new("TaskAPI"),
                new("AccountAPI"),
            };
        }
    }
}