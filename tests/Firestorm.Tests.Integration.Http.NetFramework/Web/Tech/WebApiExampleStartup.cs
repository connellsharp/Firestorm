using System.Web.Http;
using Firestorm.AspNetWebApi2;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Firestorm.Testing.Http;
using Firestorm.Tests.Integration.Http.NetFramework.Web;
using JetBrains.Annotations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApiExampleStartup))]

namespace Firestorm.Tests.Integration.Http.NetFramework.Web
{
    public class WebApiExampleStartup
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