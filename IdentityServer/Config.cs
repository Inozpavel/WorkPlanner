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
            };

        public static IEnumerable<Client> GetConfiguredClients(IConfiguration configuration) =>
            new List<Client>
            {
                new()
                {
                    ClientId = "SwaggerApp",
                    ClientSecrets =
                    {
                        new Secret(configuration["Clients:SwaggerApp:Secret"].ToSha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins =
                    {
                        configuration["Clients:SwaggerApp:Origin1"],
                        configuration["Clients:SwaggerApp:Origin2"]
                    },
                    RequireClientSecret = true,
                    AllowedScopes =
                    {
                        "IdentityServer",
                        IdentityServerConstants.StandardScopes.OpenId,
                    }
                },
                new()
                {
                    ClientId = "Gateway",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins =
                    {
                        configuration["Clients:Gateway:Origin"]
                    },
                    RequireClientSecret = true,
                    ClientSecrets =
                    {
                        new Secret(configuration["Clients:Gateway:Secret"].ToSha256())
                    },
                    AllowedScopes =
                    {
                        "IdentityServer",
                        IdentityServerConstants.StandardScopes.OpenId,
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
                        "FullName",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    }
                },
            };

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.Email(),
                new IdentityResources.OpenId(),
                new("FullName", new List<string>
                {
                    "full_name"
                })
            };
        }
    }
}