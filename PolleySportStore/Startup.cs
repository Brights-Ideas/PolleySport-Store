using Microsoft.Owin;
using Owin;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;


[assembly: OwinStartup(typeof(Api.Startup))]

namespace Store.Management.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // web api configuration
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }
}