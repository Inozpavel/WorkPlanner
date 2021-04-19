using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new("TaskAPI"),
                new("AccountAPI"),
                new("IdentityServer"),
                new(IdentityServerConstants.StandardScopes.OpenId),
                new(IdentityServerConstants.StandardScopes.Profile),
            };

        public static IEnumerable<Client> GetConfiguredClients(IConfiguration configuration) =>
            new List<Client>
            {
                new()
                {
                    ClientId = "TasksSwaggerApp",
                    ClientSecrets =
                    {
                        new Secret(configuration["SwaggerApp:Secret"].ToSha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins =
                    {
                        configuration["SwaggerApp:Origin"]
                    },
                    RequireClientSecret = true,
                    AllowedScopes =
                    {
                        "IdentityServer",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                },
                new()
                {
                    ClientId = "Gateway",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins =
                    {
                        "http://localhost:5000"
                    },
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        "IdentityServer",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                },
                new()
                {
                    ClientId = "MobileApp",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        "TaskAPI",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
            };
    }
}