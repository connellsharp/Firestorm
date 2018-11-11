using Firestorm.Endpoints;
using Firestorm.Owin;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Web;
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
            app.UseFirestorm(c =>
            {
                c.AddEndpoints(new RestEndpointConfiguration
                {
                    ResponseConfiguration =
                    {
                        ShowDeveloperErrors = true
                    }
                });
                
                c.
                new FirestormConfiguration
                {
                    EndpointConfiguration =
                    {
                    },
                    StartResourceFactory = new DoubleTestStartResourceFactory("http://localhost:" + Port)
                };
            });
        }

        public const int Port = 1113;
    }
}
