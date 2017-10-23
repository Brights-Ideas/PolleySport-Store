using Identity.Web;

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
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Web.Helpers;
using Identity.Web.Stores;

[assembly: OwinStartup(typeof(Startup))]

namespace Identity.Web
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
                        EnablePostSignOutAutoRedirect = true
                        //IdentityProviders = ConfigureIdentityProviders
                    }

                });
            });

            //app.UseResourceAuthorization(new AuthorizationManager());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44383/identity",
                ClientId = "mvc",
                Scope = "openid profile roles sampleApi",
                RedirectUri = "http://localhost:54602/",
                ResponseType = "id_token token",

                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                         // we want to keep first name, last name, subject and roles
                        //var givenName = id.FindFirst(Constants.ClaimTypes.GivenName);
                        //var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName);
                        //var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                        //var roles = id.FindAll(Constants.ClaimTypes.Role);

                         // create new identity and set name and role claim type
                         var nid = new ClaimsIdentity(
                            id.AuthenticationType,
                            Constants.ClaimTypes.GivenName,
                            Constants.ClaimTypes.Role);

                        // get userinfo data
                        var userInfoClient = new UserInfoClient(n.Options.Authority + "/connect/userinfo");//,
                                //n.ProtocolMessage.AccessToken);

                        var userInfo = await userInfoClient.GetAsync(n.ProtocolMessage.Token);
                        userInfo.Claims.ToList().ForEach(ui => nid.AddClaim(new Claim(ui.ValueType, ui.Value)));

                        //nid.AddClaim(givenName);
                        //nid.AddClaim(familyName);
                        //nid.AddClaim(sub);
                        //nid.AddClaims(roles);

                        // keep the id_token for logout
                        nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        // add access token for sample API
                        nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

                        // keep track of access token expiration
                        nid.AddClaim(new Claim("expires_at", DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));

                        // add some other app specific claim
                        nid.AddClaim(new Claim("app_specific", "some data"));

                        n.AuthenticationTicket = new AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);

                        //return Task.FromResult(0);

                    },

                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }
                        }

                        return Task.FromResult(0);
                    }
                }
            });

        }

        //private void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        //{
        //    app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
        //    {
        //        AuthenticationType = "Google",
        //        Caption = "Sign-in with Google",
        //        SignInAsAuthenticationType = signInAsType,

        //        ClientId = "91033992413-jje1duc62cgc4747o215escgfnglm83d.apps.googleusercontent.com",
        //        ClientSecret = "6zLwk4-AmOsM7xeqGinxEsFT"
        //    });
        //}

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\identityServer\idsrv3test.pfx", "idsrv3test");
        }
    }
}