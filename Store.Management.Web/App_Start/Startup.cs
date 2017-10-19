using Store.Management.Web.IdentityServer;

using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Web.Helpers;
using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityModel.Client;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Store.Management.Web.App_Start.Startup))]

namespace Store.Management.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            //JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = "Cookies"
            //});

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44383/identity",
                ClientId = "mvc",
                RedirectUri = "https://localhost:44383/",
                ResponseType = "id_token",

                SignInAsAuthenticationType = "Cookies"
            });

        }

        //X509Certificate2 LoadCertificate()
        //{
        //    return new X509Certificate2(
        //        string.Format(@"{0}\bin\identityServer\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        //}
    }
}