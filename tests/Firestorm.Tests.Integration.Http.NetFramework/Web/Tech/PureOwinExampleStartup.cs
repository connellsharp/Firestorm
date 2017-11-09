using Firestorm.Endpoints;
using Firestorm.Endpoints.Owin;
using Firestorm.Endpoints.Start;
using Firestorm.Tests.HttpWebStacks.Web;
using Firestorm.Tests.Integration.Http.Base;
using JetBrains.Annotations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PureOwinExampleStartup))]

namespace Firestorm.Tests.HttpWebStacks.Web
{
    public class PureOwinExampleStartup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            app.UseFirestorm(new FirestormConfiguration
            {
                StartResourceFactory = new IntegratedStartResourceFactory()
            });
        }
    }
}
