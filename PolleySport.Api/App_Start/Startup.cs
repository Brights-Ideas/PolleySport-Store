using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Autofac;
using System.Reflection;
using PolleySport.Api.Autofac.Modules;
using Autofac.Integration.WebApi;

[assembly: OwinStartup(typeof(PolleySport.Api.App_Start.Startup))]

namespace PolleySport.Api.App_Start
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

            app.UseWebApi(config);
        }
    }
}
