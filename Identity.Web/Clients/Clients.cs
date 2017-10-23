using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace Identity.Web
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "MVC Client",
                    ClientId = "mvc",
                    Flow = Flows.Implicit,

                    RedirectUris = new List<string>
                    {
                        "http://localhost:54602/"
                    },
                    //PostLogoutRedirectUris = new List<string>
                    //{
                    //    "https://localhost:44383/"
                    //},
                    //AllowAccessToAllScopes
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "profile",
                        "roles",
                        "sampleApi"
                    }
                },
                new Client
                {
                    ClientName = "MVC Client (service communication)",
                    ClientId = "mvc_service",
                    Flow = Flows.ClientCredentials,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "sampleApi"
                    }
                }
            };
        }
    }
}