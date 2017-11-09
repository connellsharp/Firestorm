using Firestorm.Endpoints;
using Firestorm.Endpoints.Owin;
using Firestorm.Endpoints.Start;
using Firestorm.Tests.HttpWebStacks.Web;
using JetBrains.Annotations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DoubleClientExampleStartup))]

namespace Firestorm.Tests.HttpWebStacks.Web
{
    public class DoubleClientExampleStartup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            app.UseFirestorm(new FirestormConfiguration
            {
                StartResourceFactory = new DoubleTestStartResourceFactory("http://localhost:" + Port)
            });
        }

        public const int Port = 1113;
    }
}
