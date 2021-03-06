﻿using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OpenIdConnect;
using Thinktecture.IdentityModel.Clients;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using PolleySport.Store.Web.AuthManager;

[assembly: OwinStartup(typeof(PolleySport.Store.Web.App_Start.Startup))]

namespace PolleySport.Store.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            app.UseResourceAuthorization(new AuthorizationManager());

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "mvc",
                Authority = "https://localhost:44383", //Authority = "https://localhost:44319/identity",
                RedirectUri = "http://localhost:63170/", //"https://localhost:44319/",
                ResponseType = "id_token token",

                Scope = "openid profile roles sampleApi",
                PostLogoutRedirectUri = "http://localhost:63170/",
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //UseTokenLifetime = false,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthorizationCodeReceived = async n =>
                    {
                        var requestResponse = await OidcClient.CallTokenEndpointAsync(
                            new Uri("https://localhost:44383/connect/token"),
                            new Uri("http://localhost:63170/"),
                            n.Code,
                            "socialnetwork_code",
                            "secret");

                        var identity = n.AuthenticationTicket.Identity;

                        identity.AddClaim(new Claim("access_token", requestResponse.AccessToken));
                        identity.AddClaim(new Claim("id_token", requestResponse.IdentityToken));
                        identity.AddClaim(new Claim("refresh_token", requestResponse.RefreshToken));

                        n.AuthenticationTicket = new AuthenticationTicket(
                            identity, n.AuthenticationTicket.Properties);
                    },
                    //SecurityTokenValidated = async n =>
                    //{
                    //    var nid = new ClaimsIdentity(
                    //        n.AuthenticationTicket.Identity.AuthenticationType,
                    //        Constants.ClaimTypes.GivenName,
                    //        Constants.ClaimTypes.Role);

                    //    // get userinfo data
                    //    var userInfoClient = new UserInfoClient(n.Options.Authority + "/connect/userinfo");

                    //    var userInfo = await userInfoClient.GetAsync(n.ProtocolMessage.AccessToken);
                    //    userInfo.Claims.ToList().ForEach(ui => nid.AddClaim(new Claim(ui.Type, ui.Value)));

                    //    // keep the id_token for logout
                    //    nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                    //    // add access token for sample API
                    //    nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

                    //    // keep track of access token expiration
                    //    nid.AddClaim(new Claim("expires_at", DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));

                    //    // add some other app specific claim
                    //    nid.AddClaim(new Claim("app_specific", "some data"));

                    //    n.AuthenticationTicket = new AuthenticationTicket(
                    //        nid,
                    //        n.AuthenticationTicket.Properties);
                    //},

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
