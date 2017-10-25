using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Firestorm.Engine.Data;

namespace Firestorm.Stems.Roots.DataSource
{
    public class DataSourceRootResourceFactory : IRootResourceFactory
    {
        public IEnumerable<Type> StemTypes { get; set; }

        public string StemsNamespace { get; set; }

        public IDataSource DataSource { get; set; }

        internal NamedTypeDictionary StemTypeDictionary;

        internal readonly ConcurrentDictionary<string, IDataSourceCollectionCreator> Creators = new ConcurrentDictionary<string, IDataSourceCollectionCreator>();

        public IEnumerable<Type> GetStemTypes()
        {
            if (DataSource == null)
                throw new StemStartSetupException("DataSource must be provided.");

            StemTypeDictionary = new AttributedSuffixedDerivedTypeDictionary(typeof(Stem), "Stem", typeof(DataSourceRootAttribute));

            if (StemTypes != null)
                StemTypeDictionary.AddTypes(StemTypes);

            if (StemsNamespace != null)
                StemTypeDictionary.AddNamespace(StemsNamespace);

            return StemTypeDictionary.GetAllTypes();
        }

        public IRestResource GetStartResource(IRootRequest request, IStemConfiguration configuration)
        {
            return new DataSourceDirectory(request, this, configuration);
        }
    }
}