using System;
using System.Threading.Tasks;
using Firestorm.Stems.Attributes;
using Firestorm.Stems.Fuel.Resolving;

namespace Firestorm.Stems.Roots.DataSource
{
    internal class DataSourceDirectory : IRestDirectory
    {
        private readonly IRootRequest _request;
        private readonly DataSourceRootResourceFactory _parentFactory;
        private readonly IStemConfiguration _configuration;

        internal DataSourceDirectory(IRootRequest request, DataSourceRootResourceFactory parentFactory, IStemConfiguration configuration)
        {
            _request = request;
            _parentFactory = parentFactory;
            _configuration = configuration;
        }

        public IRestResource GetChild(string startResourceName)
        {
            var autoActivator = new AutoActivator(_configuration.DependencyResolver);
            Type stemType = _parentFactory.StemTypeDictionary.GetType(startResourceName);
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

            IDataSourceCollectionCreator creator = _parentFactory.Creators.GetOrAdd(startResourceName, delegate
            {
                Type creatorType = typeof(DataSourceCollectionCreator<>).MakeGenericType(entityType);
                return (IDataSourceCollectionCreator)Activator.CreateInstance(creatorType, _parentFactory.DataSource);
            });

            return creator.GetRestCollection(stem);
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            return Task.FromResult(_parentFactory.StemTypeDictionary.CreateDirectoryInfo(_configuration.NamingConventionSwitcher));
        }
    }
}