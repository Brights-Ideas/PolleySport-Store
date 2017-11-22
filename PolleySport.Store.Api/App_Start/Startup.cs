using Microsoft.Owin;
using Owin;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using Autofac;
using System.Reflection;
using PolleySport.Store.Api.Autofac.Modules;
using Autofac.Integration.WebApi;

[assembly: OwinStartup(typeof(PolleySport.Store.Api.Startup))]

namespace PolleySport.Store.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule<PolleySportModule>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44383"///identity",
                //RequiredScopes = new[] { "sampleApi" }
            });

            // add app local claims per request
            app.UseClaimsTransformation(incoming =>
            {
                //either add claims to incoming, or create new principal
                var appPrincipal = new ClaimsPrincipal(incoming);
                incoming.Identities.First().AddClaim(new Claim("appSpecific", "some_value"));

                return Task.FromResult(appPrincipal);
            });

            // web api configuration
            //var config = new HttpConfiguration();
            //config.MapHttpAttributeRoutes();

            app.UseWebApi(config);

        }
    }
}