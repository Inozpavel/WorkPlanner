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
                        new Secret(configuration["SwaggerApp:Secret"].ToSha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins =
                    {
                        configuration["SwaggerApp:Origin1"],
                        configuration["SwaggerApp:Origin2"]
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
                        "http://localhost:5000"
                    },
                    RequireClientSecret = false,
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
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        "FullName"
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