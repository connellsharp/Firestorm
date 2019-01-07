using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;
using Firestorm.Endpoints.Formatting.Json;
using Firestorm.Endpoints.Formatting.Xml;
using Firestorm.AspNetWebApi2.ErrorHandling;
using Firestorm.Defaults;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
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
        public static void SetupFirestorm(this HttpConfiguration httpConfig, Action<IFirestormServicesBuilder> configureAction)
        {
            SetupFirestorm(httpConfig, null, configureAction);
        }

        /// <summary>Maps the specified route template for a REST API running within Web API.</summary>
        /// <returns>A reference to the mapped route.</returns>
        /// <param name="httpConfig">The Web API configuration.</param>
        /// <param name="directory">The starting directory of the REST API.</param>
        /// <param name="configureAction">The Firestorm configuration.</param>
        public static void SetupFirestorm(this HttpConfiguration httpConfig, string directory, Action<IFirestormServicesBuilder> configureAction)
        {
            httpConfig.Routes.MapFirestorm(directory, configureAction);

            //httpConfig.Filters.Add(new RestApiExceptionFilterAttribute()); // global filter
            //config.Services.Replace(typeof(IExceptionHandler), new OopsExceptionHandler()); // global handler

            FirestormController.GlobalConfig.EndpointConfiguration.EnsureValid();
            var nameSwitcher = FirestormController.GlobalConfig.EndpointConfiguration.NamingConventionSwitcher;

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
        public static IHttpRoute MapFirestorm(this HttpRouteCollection routes, string directory, Action<IFirestormServicesBuilder> configureAction)
        {
            var servicesBuilder = new DefaultServicesBuilder();
            configureAction(servicesBuilder);
            var serviceProvider = servicesBuilder.Build();

            var config = new FirestormConfiguration
            {
                EndpointConfiguration = serviceProvider.GetService<RestEndpointConfiguration>(),
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
