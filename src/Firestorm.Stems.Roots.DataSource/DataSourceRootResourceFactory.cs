using System;
using System.Collections.Generic;
using Firestorm.Engine.Data;

namespace Firestorm.Stems.Roots.DataSource
{
    public class DataSourceRootResourceFactory : IRootResourceFactory
    {
        public IEnumerable<Type> StemTypes { get; set; }

        public string StemsNamespace { get; set; }

        public IDataSource DataSource { get; set; }

        private NamedTypeDictionary _stemTypeDictionary;

        private readonly CollectionCreatorCache _creators = new CollectionCreatorCache();

        public IEnumerable<Type> GetStemTypes()
        {
            if (DataSource == null)
                throw new StemStartSetupException("DataSource must be provided.");

            _stemTypeDictionary = new AttributedSuffixedDerivedTypeDictionary(typeof(Stem), "Stem", typeof(DataSourceRootAttribute));

            if (StemTypes != null)
                _stemTypeDictionary.AddTypes(StemTypes);

            if (StemsNamespace != null)
                _stemTypeDictionary.AddNamespace(StemsNamespace);

            return _stemTypeDictionary.GetAllTypes();
        }

        public IRestResource GetStartResource(IRootRequest request, IStemConfiguration configuration)
        {
            return new DataSourceDirectory(configuration, request, _stemTypeDictionary, _creators, DataSource);
        }
    }
}