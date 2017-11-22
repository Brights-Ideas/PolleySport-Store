using Autofac;
using PolleySport.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PolleySport.Store.Api.Autofac.Modules
{
    public class PolleySportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new Func<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["BrightIdeas"].ConnectionString)));

            builder.RegisterType<UserRepository>().AsImplementedInterfaces().AsSelf();
            //builder.RegisterType<ProfileRepository>().AsImplementedInterfaces().AsSelf();
            //builder.RegisterType<ShoutRepository>().AsImplementedInterfaces().AsSelf();
        }
    }
}