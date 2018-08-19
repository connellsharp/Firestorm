using System;
using System.Collections.Generic;

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

        public IRestResource GetStartResource(IStemConfiguration configuration, IRootRequest request)
        {
            return new RootsDirectory(configuration, _startInfoFactory, request, _creators);
        }
    }
}