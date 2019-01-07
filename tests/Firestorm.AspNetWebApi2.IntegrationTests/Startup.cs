using System.Web.Http;
using Firestorm.AspNetWebApi2.IntegrationTests;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Firestorm.Testing.Http;
using JetBrains.Annotations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Firestorm.AspNetWebApi2.IntegrationTests
{
    public class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.SetupFirestorm(c =>
            {
                c.AddEndpoints();
                c.AddStartResourceFactory(new IntegratedStartResourceFactory());
            });

            app.UseWebApi(config);
        }
    }
}