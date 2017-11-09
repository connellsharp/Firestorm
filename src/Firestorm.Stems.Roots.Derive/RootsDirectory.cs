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
        private readonly DerivedRootsResourceFactory _parentFactory;
        private readonly IRootRequest _request;
        private readonly IStemConfiguration _configuration;

        public RootsDirectory(IRootRequest request, DerivedRootsResourceFactory parentFactory, IStemConfiguration configuration)
        {
            _request = request;
            _parentFactory = parentFactory;
            _configuration = configuration;
        }

        public IRestResource GetChild(string startResourceName)
        {
            var autoActivator = new AutoActivator(_configuration.DependencyResolver);

            Type rootType = _parentFactory.RootTypeDictionary.GetType(startResourceName);
            var root = (Root)autoActivator.CreateInstance(rootType);

            root.Configuration = _configuration;
            root.User = _request.User;
            _request.OnDispose += delegate { root.Dispose(); };

            var stem = (Stem)autoActivator.CreateInstance(root.StartStemType);
            stem.SetParent(root);
            
            IRootCollectionCreator creator = _parentFactory.Creators.GetOrAdd(startResourceName, delegate
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
            return Task.FromResult(_parentFactory.RootTypeDictionary.CreateDirectoryInfo(_configuration.NamingConventionSwitcher));
        }
    }
}