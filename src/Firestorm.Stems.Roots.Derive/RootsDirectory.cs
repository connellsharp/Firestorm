using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Firestorm.Stems.Attributes;
using Firestorm.Stems.Fuel.Resolving;

namespace Firestorm.Stems.Roots.Derive
{
    /// <summary>
    /// Contains the user configuration for Stems and creates starting collections from <see cref="Root"/> implementations.
    /// </summary>
    public class RootsDirectory : IRestDirectory
    {
        private readonly IStemConfiguration _configuration;
        private readonly IRootRequest _request;
        private readonly NamedTypeDictionary _rootTypeDictionary;
        private readonly CollectionCreatorCache _creatorCache;

        public RootsDirectory(IStemConfiguration configuration, IRootRequest request, NamedTypeDictionary rootTypeDictionary)
            : this(configuration, request, rootTypeDictionary, new CollectionCreatorCache())
        { }

        internal RootsDirectory(IStemConfiguration configuration, IRootRequest request, NamedTypeDictionary rootTypeDictionary, CollectionCreatorCache creatorCache)
        {
            _request = request;
            _rootTypeDictionary = rootTypeDictionary;
            _creatorCache = creatorCache;
            _configuration = configuration;
        }

        public IRestResource GetChild(string startResourceName)
        {
            var autoActivator = new AutoActivator(_configuration.DependencyResolver);

            Type rootType = _rootTypeDictionary.GetType(startResourceName);
            var root = (Root)autoActivator.CreateInstance(rootType);

            root.Configuration = _configuration;
            root.User = _request.User;
            _request.OnDispose += delegate { root.Dispose(); };

            if (root.StartStemType == null)
                throw new ArgumentNullException(nameof(root.StartStemType), "Root class StartStemType cannot be null.");

            var stem = (Stem)autoActivator.CreateInstance(root.StartStemType);
            stem.SetParent(root);
            
            IRootCollectionCreator creator = _creatorCache.GetOrAdd(startResourceName, delegate
            {
                Type genericRootType = rootType.GetGenericSubclass(typeof(Root<>));
                Debug.Assert(genericRootType != null, "Weakly-typed root was somehow created.");
                Type itemType = genericRootType.GenericTypeArguments[0];
                Type creatorType = typeof(RootCollectionCreator<>).MakeGenericType(itemType);
                return (IRootCollectionCreator)Activator.CreateInstance(creatorType);
            });

            return creator.GetRestCollection(root, stem);
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            return Task.FromResult(_rootTypeDictionary.CreateDirectoryInfo(_configuration.NamingConventionSwitcher));
        }
    }
}