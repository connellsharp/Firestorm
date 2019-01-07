using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Firestorm.Owin.IntegrationTests;
using Firestorm.Testing.Http;
using JetBrains.Annotations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Firestorm.Owin.IntegrationTests
{
    public class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            app.UseFirestorm(c => c
                .AddEndpoints()
                .AddStartResourceFactory(new IntegratedStartResourceFactory())
            );
        }
    }
}
