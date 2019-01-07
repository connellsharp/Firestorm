using System;
using System.Collections.Generic;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Stems.Roots.Combined
{
    public abstract class RootResourceFactoryBase : IRootResourceFactory
    {
        private readonly CollectionCreatorCache _creators = new CollectionCreatorCache();
        private IRootStartInfoFactory _startInfoFactory;

        protected abstract IRootStartInfoFactory CreateStartInfoFactory();

        public IEnumerable<Type> GetStemTypes(IStemConfiguration configuration)
        {
            _startInfoFactory = CreateStartInfoFactory();

            return _startInfoFactory.GetStemTypes(configuration);
        }

        public IRestResource GetStartResource(IStemConfiguration configuration, IRequestContext requestContext)
        {
            return new RootsDirectory(configuration, _startInfoFactory, requestContext, _creators);
        }
    }
}