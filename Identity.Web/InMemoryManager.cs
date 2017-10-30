using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Identity.Web
{
    public class InMemoryManager
    {
        public IEnumerable<Scope> GetScopes()
        {
            var scopes = new List<Scope>
        {
            new Scope
            {
                Enabled = true,
                Name = "roles",
                Type = ScopeType.Identity,
                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim("role")
                }
            },
            new Scope
            {
                Enabled = true,
                DisplayName = "Sample API",
                Name = "sampleApi",
                Description = "Access to a sample API",
                Type = ScopeType.Resource,

                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim("role")
                }
            },
            new Scope
            {
                Enabled = true,
                DisplayName = "Read User Data",//DisplayName = "Sample API",
                Name = "read",//Name = "sampleApi",
                Description = "Access to a sample API",
                Type = ScopeType.Resource,

                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim("role")
                }
            }
        };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }

        public IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "MVC Client",
                    ClientId = "mvc",
                    Flow = Flows.Implicit,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = new List<string>
                    {
                        "http://localhost:63170/"
                    },
                    //PostLogoutRedirectUris = new List<string>
                    //{
                    //    "https://localhost:44383/"
                    //},
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "profile",
                        "roles",
                        "sampleApi",
                        "read"
                    }
                },
                new Client
                {
                    Enabled = true,
                    ClientName = "MVC Client (service communication)",
                    ClientId = "mvc_service",
                    Flow = Flows.ResourceOwner,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        "read"
                    }
                }
            };
        }
    }
}