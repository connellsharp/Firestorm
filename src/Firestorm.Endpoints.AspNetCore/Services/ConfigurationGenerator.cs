using System;
using System.Collections.Generic;
using Firestorm.Endpoints.Query;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Strategies;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Firestorm.Endpoints.AspNetCore
{
    /// <summary>
    /// Generates a <see cref="FirestormConfiguration"/> object based on the configured services and options.
    /// Can use the ASP.NET Core IoC container and/or configuring <see cref="Action"/>s.
    /// </summary>
    internal class ConfigurationGenerator
    {
        private readonly IServiceProvider _services;
        private readonly List<Action<FirestormConfiguration>> _configureActions;

        public ConfigurationGenerator([CanBeNull] IServiceProvider services)
        {
            _services = services;
            _configureActions = new List<Action<FirestormConfiguration>>();
        }

        internal void Configure([NotNull] Action<FirestormConfiguration> configureAction)
        {
            if (configureAction == null)
                throw new ArgumentNullException(nameof(configureAction));

            _configureActions.Add(configureAction);
        }

        internal FirestormConfiguration Create()
        {
            FirestormConfiguration config = GetInjectedConfig();
            
            foreach (Action<FirestormConfiguration> configureAction in GetConfigureActions())
            {
                configureAction(config);
            }

            return config;
        }

        private FirestormConfiguration GetInjectedConfig()
        {
            if(_services == null)
                return new FirestormConfiguration();

            return new FirestormConfiguration
            {
                StartResourceFactory = _services.GetService<IStartResourceFactory>(),
                EndpointConfiguration = new RestEndpointConfiguration
                {
                    QueryStringConfiguration = _services.GetService<CollectionQueryStringConfiguration>(),
                    ResponseContentGenerator = _services.GetService<IResponseContentGenerator>(),
                    RequestStrategies = _services.GetService<UnsafeRequestStrategySets>(),
                }
            };
        }

        private IEnumerable<Action<FirestormConfiguration>> GetConfigureActions()
        {
            var configureServices = _services.GetServices<IConfigureOptions<FirestormConfiguration>>();
            foreach (IConfigureOptions<FirestormConfiguration> configureOptions in configureServices)
            {
                yield return configureOptions.Configure;
            }

            foreach(Action<FirestormConfiguration> action in _configureActions)
            {
                yield return action;
            }
        }
    }
}