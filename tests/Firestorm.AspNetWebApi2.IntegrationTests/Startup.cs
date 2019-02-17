using System.Web.Http;
using Firestorm.AspNetWebApi2.IntegrationTests;
using Firestorm.Endpoints;
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
                c.AddEndpoints(e =>
                {
                    e.Response.ShowDeveloperErrors = true;
                    //
                });
                c.AddStartResourceFactory(new IntegratedStartResourceFactory());
            });

            app.UseWebApi(config);
        }
    }
}