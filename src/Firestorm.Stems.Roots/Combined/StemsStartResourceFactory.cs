using System;
using System.Collections.Generic;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Stems.Roots.Combined
{
    /// <summary>
    /// Contains the user configuration for Stems and creates starting collection Stem types from an <see cref="IRootResourceFactory"/> implementation.
    /// </summary>
    public class StemsStartResourceFactory : IStartResourceFactory
    {
        private bool _initialized = false;
        private readonly IStemsCoreServices _services;
        private readonly IRootStartInfoFactory _startInfoFactory;
        private readonly CollectionCreatorCache _creators = new CollectionCreatorCache();

        public StemsStartResourceFactory(IStemsCoreServices services, IRootStartInfoFactory startInfoFactory)
        {
            _services = services;
            _startInfoFactory = startInfoFactory;
        }

        public void Initialize()
        {
            IEnumerable<Type> stemTypes = _startInfoFactory.GetStemTypes(_services);

            _services.ServiceGroup?.Preload(stemTypes);

            _initialized = true;
        }

        public IRestResource GetStartResource(IRequestContext hostContext)
        {
            if(!_initialized)
                Initialize();

            return new RootsDirectory(_services, _startInfoFactory, hostContext, _creators);
        }
    }
}