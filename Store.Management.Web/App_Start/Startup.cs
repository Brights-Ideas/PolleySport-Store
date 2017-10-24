using IdentityModel.Client;
using IdentityServer3.Core;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Store.Management.Web;
using Store.Management.Web.AuthManager;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Startup))]

namespace Store.Management.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseResourceAuthorization(new AuthorizationManager());

            //app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            //{
            //    Authority = "https://localhost:44383/identity",
            //    ClientId = "mvc",
            //    Scope = "openid profile roles",
            //    RedirectUri = "http://localhost:54602/",
            //    ResponseType = "id_token",

            //    SignInAsAuthenticationType = "Cookies"
            //});

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44383/identity", //Authority = "https://localhost:44319/identity",

                ClientId = "mvc",
                Scope = "openid profile roles sampleApi",
                ResponseType = "id_token token",
                RedirectUri = "http://localhost:54602/", //"https://localhost:44319/",

                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async n =>
                    {
                        var nid = new ClaimsIdentity(
                            n.AuthenticationTicket.Identity.AuthenticationType,
                            Constants.ClaimTypes.GivenName,
                            Constants.ClaimTypes.Role);

                        // get userinfo data
                        var userInfoClient = new UserInfoClient(n.Options.Authority + "/connect/userinfo");
                            //new Uri(n.Options.Authority + "/connect/userinfo"),
                            //n.ProtocolMessage.AccessToken);

                        var userInfo = await userInfoClient.GetAsync(n.ProtocolMessage.AccessToken);
                        userInfo.Claims.ToList().ForEach(ui => nid.AddClaim(new Claim(ui.Type, ui.Value)));

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

    }
}