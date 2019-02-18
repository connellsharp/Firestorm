using System;
using System.Collections.Generic;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Roots;

namespace Firestorm.Stems
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
        public StemsServices StemsServices { get; set; } = new StemsServices();

        /// <summary>
        /// Defines which Stem <see cref="Type"/>s are used and how to get the start resource.
        /// </summary>
        public IRootResourceFactory RootResourceFactory { get; set; }

        public void Initialize()
        {
            IEnumerable<Type> stemTypes = RootResourceFactory.GetStemTypes(StemsServices);

            StemsServices.ServiceGroup.Preload(stemTypes);

            _initialized = true;
        }

        public IRestResource GetStartResource(IRequestContext hostContext)
        {
            if(!_initialized)
                Initialize();

            return RootResourceFactory.GetStartResource(StemsServices, hostContext);
        }
    }
}