﻿using Store.Management.Web.IdentityServer;

using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.Google;
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
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.Map("/identity", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Embedded IdentityServer",
                    SigningCertificate = LoadCertificate(),

                    Factory = new IdentityServerServiceFactory()
                                .UseInMemoryUsers(Users.Get())
                                .UseInMemoryClients(Clients.Get())
                                .UseInMemoryScopes(Scopes.Get()),

                    AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions
                    {
                        EnablePostSignOutAutoRedirect = true,
                        //IdentityProviders = ConfigureIdentityProviders
                    }

                });
            });

            app.UseResourceAuthorization(new AuthorizationManager());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44306/identity",

                ClientId = "mvc",
                Scope = "openid profile roles",
                RedirectUri = "https://localhost:44306/",
                ResponseType = "id_token",

                SignInAsAuthenticationType = "Cookies",

                 Notifications = new OpenIdConnectAuthenticationNotifications
                 {
                     SecurityTokenValidated = n =>
                     {
                         var id = n.AuthenticationTicket.Identity;

                         // we want to keep first name, last name, subject and roles
                         var givenName = id.FindFirst(Constants.ClaimTypes.GivenName);
                         var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName);
                         var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                         var roles = id.FindAll(Constants.ClaimTypes.Role);

                         // create new identity and set name and role claim type
                         var nid = new ClaimsIdentity(
                             id.AuthenticationType,
                             Constants.ClaimTypes.GivenName,
                             Constants.ClaimTypes.Role);

                         nid.AddClaim(givenName);
                         nid.AddClaim(familyName);
                         nid.AddClaim(sub);
                         nid.AddClaims(roles);

                         // add some other app specific claim
                         nid.AddClaim(new Claim("app_specific", "some data"));

                         n.AuthenticationTicket = new AuthenticationTicket(
                             nid,
                             n.AuthenticationTicket.Properties);

                         return Task.FromResult(0);

                     }
                 }
            });
            
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\identityServer\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }
    }
}