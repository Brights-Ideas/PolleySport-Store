using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Autofac;
using PolleySport.Data.Repositories;

namespace SocialNetwork.Api.Autofac.Modules
{
    public class SocialNetworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new Func<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["BrightsIdeas"].ConnectionString)));

            builder.RegisterType<UserRepository>().AsImplementedInterfaces().AsSelf();
            //builder.RegisterType<ProfileRepository>().AsImplementedInterfaces().AsSelf();
            //builder.RegisterType<ShoutRepository>().AsImplementedInterfaces().AsSelf();
        }
    }
}