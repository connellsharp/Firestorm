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

        internal NamedTypeDictionary RootTypeDictionary;

        internal readonly ConcurrentDictionary<string, IRootCollectionCreator> Creators = new ConcurrentDictionary<string, IRootCollectionCreator>();

        public IEnumerable<Type> GetStemTypes()
        {
            RootTypeDictionary = new SuffixedDerivedTypeDictionary(typeof(Root), "Root");

            if (RootTypes != null)
                RootTypeDictionary.AddTypes(RootTypes);

            if (RootsNamespace != null)
                RootTypeDictionary.AddNamespace(RootsNamespace);

            return RootTypeDictionary.GetAllTypes().Select(GetStemFromRoot);
        }

        private static Type GetStemFromRoot(Type rootType)
        {
            var root = (Root)Activator.CreateInstance(rootType);
            return root.StartStemType;
        }

        public IRestResource GetStartResource(IRootRequest request, IStemConfiguration configuration)
        {
            return new RootsDirectory(request, this, configuration);
        }
    }
}