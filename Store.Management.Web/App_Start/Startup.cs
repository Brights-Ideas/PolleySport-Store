using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Store.Management.Web;
using Store.Management.Web.AuthManager;

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

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44383/identity",
                ClientId = "mvc",
                Scope = "openid profile roles",
                RedirectUri = "http://localhost:54602/",
                ResponseType = "id_token",

                SignInAsAuthenticationType = "Cookies"
            });

        }

    }
}