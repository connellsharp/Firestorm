using Firestorm.Owin;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Firestorm.Testing.Http;
using Firestorm.Tests.Integration.Http.NetFramework.Web;
using JetBrains.Annotations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PureOwinExampleStartup))]

namespace Firestorm.Tests.Integration.Http.NetFramework.Web
{
    public class PureOwinExampleStartup
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
