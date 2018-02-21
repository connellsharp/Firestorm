using System;
using System.Collections.Generic;

namespace Firestorm.Stems.Roots.Combined
{
    public abstract class RootResourceFactoryBase : IRootResourceFactory
    {
        private readonly CollectionCreatorCache _creators = new CollectionCreatorCache();
        private IRootStartInfoFactory _startInfoFactory;

        public IEnumerable<Type> GetStemTypes()
        {
            _startInfoFactory = CreateStartInfoFactory();

            return _startInfoFactory.GetStemTypes();
        }

        protected abstract IRootStartInfoFactory CreateStartInfoFactory();

        public IRestResource GetStartResource(IRootRequest request, IStemConfiguration configuration)
        {
            return new RootsDirectory(configuration, _startInfoFactory, request, _creators);
        }
    }
}