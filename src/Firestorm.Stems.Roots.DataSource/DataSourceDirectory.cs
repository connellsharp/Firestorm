using System;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Stems.Attributes;
using Firestorm.Stems.Fuel.Resolving;

namespace Firestorm.Stems.Roots.DataSource
{
    internal class DataSourceDirectory : IRestDirectory
    {
        private readonly IRootRequest _request;
        private readonly NamedTypeDictionary _stemTypeDictionary;
        private readonly IStemConfiguration _configuration;
        private readonly CollectionCreatorCache _creatorCache;
        private readonly IDataSource _dataSource;

        internal DataSourceDirectory(IStemConfiguration configuration, IRootRequest request, NamedTypeDictionary stemTypeDictionary, CollectionCreatorCache creatorCache, IDataSource dataSource)
        {
            _configuration = configuration;
            _request = request;
            _stemTypeDictionary = stemTypeDictionary;
            _creatorCache = creatorCache;
            _dataSource = dataSource;
        }

        public IRestResource GetChild(string startResourceName)
        {
            var autoActivator = new AutoActivator(_configuration.DependencyResolver);
            Type stemType = _stemTypeDictionary.GetType(startResourceName);
            var stem = (Stem)autoActivator.CreateInstance(stemType);

            Type stemGenericBaseType = stemType.GetGenericSubclass(typeof(Stem<>));
            if (stemGenericBaseType == null)
                throw new StemStartSetupException("The Stem class does not derive from Stem<>.");

            Type entityType = stemGenericBaseType.GenericTypeArguments[0];

            var vase = new Vase
            {
                Configuration = _configuration,
                User = _request.User
            };

            _request.OnDispose += delegate { vase.Dispose(); };

            stem.SetParent(vase);

            IDataSourceCollectionCreator creator = _creatorCache.GetOrAdd(startResourceName, delegate
            {
                Type creatorType = typeof(DataSourceCollectionCreator<>).MakeGenericType(entityType);
                return (IDataSourceCollectionCreator)Activator.CreateInstance(creatorType, _dataSource);
            });

            return creator.GetRestCollection(stem);
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            return Task.FromResult(_stemTypeDictionary.CreateDirectoryInfo(_configuration.NamingConventionSwitcher));
        }
    }
}