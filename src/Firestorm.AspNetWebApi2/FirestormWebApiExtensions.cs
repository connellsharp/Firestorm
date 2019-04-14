using System;
using System.Web.Http;
using System.Web.Http.Routing;
using Firestorm.Endpoints.Formatting.Json;
using Firestorm.Endpoints.Formatting.Xml;
using Firestorm.AspNetWebApi2.ErrorHandling;
using Firestorm.Endpoints;
using Firestorm.Host;
using Newtonsoft.Json;

namespace Firestorm.AspNetWebApi2
{
    public static class FirestormWebApiExtensions
    {
        /// <summary>Maps the specified route template for a REST API running within Web API.</summary>
        /// <returns>A reference to the mapped route.</returns>
        /// <param name="httpConfig">The Web API configuration.</param>
        /// <param name="configureAction">The Firestorm configuration.</param>
        public static void SetupFirestorm(this HttpConfiguration httpConfig, Action<IServicesBuilder> configureAction)
        {
            SetupFirestorm(httpConfig, null, configureAction);
        }

        /// <summary>Maps the specified route template for a REST API running within Web API.</summary>
        /// <returns>A reference to the mapped route.</returns>
        /// <param name="httpConfig">The Web API configuration.</param>
        /// <param name="directory">The starting directory of the REST API.</param>
        /// <param name="configureAction">The Firestorm configuration.</param>
        public static void SetupFirestorm(this HttpConfiguration httpConfig, string directory, Action<IServicesBuilder> configureAction)
        {
            httpConfig.Routes.MapFirestorm(directory, configureAction);

            //httpConfig.Filters.Add(new RestApiExceptionFilterAttribute()); // global filter
            //config.Services.Replace(typeof(IExceptionHandler), new OopsExceptionHandler()); // global handler

            var nameSwitcher = FirestormController.GlobalConfig.EndpointServices.NameSwitcher;

            httpConfig.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new NameSwitcherContractResolver(nameSwitcher),
                Converters = { new ResourceBodyJsonConverter(nameSwitcher) }
            };

            // TODO: xml serialization doesn't work..
            //httpConfig.Formatters.XmlFormatter.SetSerializer<RestItemData>(new RestItemDataXmlSerializer());
            //httpConfig.Formatters.XmlFormatter.SetSerializer<IEnumerable<RestItemData>>(new RestItemDataXmlSerializer());
        }

        /// <summary>Maps the specified route template for a Firestorm REST API running within Web API.</summary>
        /// <returns>A reference to the mapped route.</returns>
        /// <param name="routes">A collection of routes for the application.</param>
        /// <param name="directory">The starting directory of the REST API.</param>
        /// <param name="configureAction">The Firestorm configuration.</param>
        public static IHttpRoute MapFirestorm(this HttpRouteCollection routes, string directory, Action<IServicesBuilder> configureAction)
        {
            var servicesBuilder = new DefaultServicesBuilder();
            configureAction(servicesBuilder);
            var serviceProvider = servicesBuilder.Build();

            var config = new FirestormConfiguration
            {
                EndpointServices = serviceProvider.GetService<EndpointServices>(),
                StartResourceFactory = serviceProvider.GetService<IStartResourceFactory>(),
            };

            FirestormController.GlobalConfig = config;

            config.StartResourceFactory.Initialize();

            return routes.MapHttpRoute(name: "Firestorm" + (directory != null ? "_" + directory : "Root"), routeTemplate: (directory != null ? directory + "/" : string.Empty) + "{*path}", defaults: new
            {
                controller = "Firestorm",
            });
        }
    }
}
