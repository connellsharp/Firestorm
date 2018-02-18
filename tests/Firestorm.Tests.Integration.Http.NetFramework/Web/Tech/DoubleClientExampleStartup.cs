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
            app.UseFirestorm(new FirestormConfiguration
            {
                EndpointConfiguration =
                {
                    ResponseConfiguration =
                    {
                        ShowDeveloperErrors = true
                    }
                },
                StartResourceFactory = new DoubleTestStartResourceFactory("http://localhost:" + Port)
            });
        }

        public const int Port = 1113;
    }
}
