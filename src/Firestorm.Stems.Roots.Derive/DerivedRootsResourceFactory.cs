using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Firestorm.Stems.Roots.Derive
{
    /// <summary>
    /// Contains the user configuration for Stems and creates starting collections from <see cref="Root"/> implementations.
    /// </summary>
    public class DerivedRootsResourceFactory : IRootResourceFactory
    {
        public IEnumerable<Type> RootTypes { get; set; }

        public string RootsNamespace { get; set; }

        private NamedTypeDictionary _rootTypeDictionary;

        private readonly CollectionCreatorCache _creators = new CollectionCreatorCache();

        public IEnumerable<Type> GetStemTypes()
        {
            _rootTypeDictionary = new SuffixedDerivedTypeDictionary(typeof(Root), "Root");

            if (RootTypes != null)
                _rootTypeDictionary.AddTypes(RootTypes);

            if (RootsNamespace != null)
                _rootTypeDictionary.AddNamespace(RootsNamespace);

            return _rootTypeDictionary.GetAllTypes().Select(GetStemFromRoot);
        }

        private static Type GetStemFromRoot(Type rootType)
        {
            var root = (Root)Activator.CreateInstance(rootType);
            return root.StartStemType;
        }

        public IRestResource GetStartResource(IRootRequest request, IStemConfiguration configuration)
        {
            return new RootsDirectory(configuration, request, _rootTypeDictionary, _creators);
        }
    }
}