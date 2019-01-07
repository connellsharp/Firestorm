using System;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Host.Infrastructure;
using Firestorm.Stems.Fuel.Resolving;
using Firestorm.Stems.Roots.Derive;
using Reflectious;

namespace Firestorm.Stems.Roots.Combined
{
    /// <summary>
    /// Contains the user configuration for Stems and creates starting collections from <see cref="Root"/> implementations.
    /// </summary>
    public class RootsDirectory : IRestDirectory
    {
        private readonly IStemConfiguration _configuration;
        private readonly CollectionCreatorCache _creatorCache;
        private readonly IRootStartInfoFactory _startInfoFactory;
        private readonly IRequestContext _request;

        public RootsDirectory(IStemConfiguration configuration, IRootStartInfoFactory startInfoFactory, IRequestContext request)
            : this(configuration, startInfoFactory, request, new CollectionCreatorCache())
        { }

        internal RootsDirectory(IStemConfiguration configuration, IRootStartInfoFactory startInfoFactory, IRequestContext request, CollectionCreatorCache creatorCache)
        {
            _configuration = configuration;
            _startInfoFactory = startInfoFactory;
            _request = request;
            _creatorCache = creatorCache;
        }

        public IRestResource GetChild(string startResourceName)
        {
            IRootStartInfo startInfo = _startInfoFactory.Get(_configuration, startResourceName);

            Type stemType = startInfo.GetStemType();
            if (stemType == null)
                throw new ArgumentNullException(nameof(stemType), "Root class StartStemType cannot be null.");

            IAxis axis = startInfo.GetAxis(_configuration, _request.User);
            _request.OnDispose += delegate { axis.Dispose(); };

            var autoActivator = new AutoActivator(_configuration.DependencyResolver);
            var stem = (Stem)autoActivator.CreateInstance(stemType);

            stem.SetParent(axis);

            IDataSourceCollectionCreator creator = _creatorCache.GetOrAdd(startResourceName, delegate
            {
                Type stemGenericBaseType = stemType.GetGenericSubclass(typeof(Stem<>));
                if (stemGenericBaseType == null)
                    throw new StemStartSetupException("The Stem class does not derive from Stem<>.");

                Type itemType = stemGenericBaseType.GenericTypeArguments[0];
                Type creatorType = typeof(DataSourceCollectionCreator<>).MakeGenericType(itemType);
                IDataSource dataSource = startInfo.GetDataSource(); // TODO same instance from first root reused?
                return (IDataSourceCollectionCreator)Activator.CreateInstance(creatorType, dataSource);
            });

            return creator.GetRestCollection(stem);
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            return Task.FromResult(_startInfoFactory.CreateDirectoryInfo());
        }
    }
}