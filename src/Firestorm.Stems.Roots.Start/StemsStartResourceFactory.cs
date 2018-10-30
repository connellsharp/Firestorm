using System;
using System.Collections.Generic;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;
using Firestorm.Host;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Contains the user configuration for Stems and creates starting collection Stem types from an <see cref="IRootResourceFactory"/> implementation.
    /// </summary>
    public class StemsStartResourceFactory : IStartResourceFactory
    {
        private bool _initialized = false;

        /// <summary>
        /// Contains the configuration for how the application's Stem objects can be utilised.
        /// </summary>
        public IStemConfiguration StemConfiguration { get; set; } = new DefaultStemConfiguration();

        /// <summary>
        /// Defines which Stem <see cref="Type"/>s are used and how to get the start resource.
        /// </summary>
        public IRootResourceFactory RootResourceFactory { get; set; }

        public void Initialize()
        {
            IEnumerable<Type> stemTypes = RootResourceFactory.GetStemTypes(StemConfiguration);

            var cacheBuilder = new AnalyzerCacheBuilder(StemConfiguration);
            cacheBuilder.AnalyzeAllStems(stemTypes);

            _initialized = true;
        }

        public IRestResource GetStartResource(IRequestContext hostContext)
        {
            if(!_initialized)
                Initialize();

            var rootRequest = new EndpointRootRequest(hostContext);

            return RootResourceFactory.GetStartResource(StemConfiguration, rootRequest);
        }
    }
}