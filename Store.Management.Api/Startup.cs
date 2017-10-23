using Microsoft.Owin;
using Owin;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;


[assembly: OwinStartup(typeof(Store.Management.Api.Startup))]

namespace Store.Management.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44383/identity",
                RequiredScopes = new []{"sampleApi"}
            });

            // web api configuration
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }
}