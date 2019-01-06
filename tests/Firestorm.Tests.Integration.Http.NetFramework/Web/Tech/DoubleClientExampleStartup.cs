using Firestorm.Endpoints;
using Firestorm.Owin;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Firestorm.Tests.Integration.Http.NetFramework.Web;
using JetBrains.Annotations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DoubleClientExampleStartup))]

namespace Firestorm.Tests.Integration.Http.NetFramework.Web
{
    public class DoubleClientExampleStartup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            app.UseFirestorm(c => c
                .AddEndpoints(new RestEndpointConfiguration
                {
                    ResponseConfiguration =
                    {
                        ShowDeveloperErrors = true
                    }
                })
                .AddStartResourceFactory(new DoubleTestStartResourceFactory("http://localhost:" + Port))
            );
        }

        public const int Port = 1113;
    }
}
