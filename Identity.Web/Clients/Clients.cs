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
                        "https://localhost:44383/"
                        //"https://localhost:44341/"
                    },
                    //PostLogoutRedirectUris = new List<string>
                    //{
                    //    "https://localhost:44383/"
                    //},

                    AllowAccessToAllScopes = true
                }
            };
        }
    }
}