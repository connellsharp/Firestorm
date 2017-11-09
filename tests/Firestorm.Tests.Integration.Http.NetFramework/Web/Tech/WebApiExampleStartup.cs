using System.Web.Http;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.WebApi2;
using Firestorm.Tests.HttpWebStacks.Web;
using Firestorm.Tests.Integration.Http.Base;
using JetBrains.Annotations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApiExampleStartup))]

namespace Firestorm.Tests.HttpWebStacks.Web
{
    public class WebApiExampleStartup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.SetupFirestorm(new FirestormConfiguration
            {
                StartResourceFactory = new IntegratedStartResourceFactory()
            });

            app.UseWebApi(config);
        }
    }
}