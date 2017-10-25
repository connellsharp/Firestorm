using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Firestorm.Endpoints;
using Firestorm.Endpoints.WebApi;
using Firestorm.Tests.Models;
using Owin;

namespace Firestorm.Tests.Console
{
    public class Startup
    {
        public const string BaseAddress = "http://localhost:9000/";

        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            //config.Filters.AddAsync(new RestApiExceptionFilterAttribute()); // global filter
            //config.Services.Replace(typeof(IExceptionHandler), new OopsExceptionHandler()); // global handler

            //FirestormController.DefaultStartResourceFactory = new IntegratedStartResourceFactory();
            //config.Routes.MapFirestorm();

            config.SetupFirestorm(new FirestormConfiguration
            {
                StartResourceFactory = new DoubleTestStartResourceFactory(BaseAddress + "rest/")
            });

            appBuilder.UseWebApi(config);
        }
    }
}