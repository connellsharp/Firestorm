using System;
using System.Collections.Generic;
using Firestorm.Data;

namespace Firestorm.Stems.Roots.DataSource
{
    public class DataSourceRootResourceFactory : IRootResourceFactory
    {
        public IDataSource DataSource { get; set; }

        public ITypeGetter StemTypeGetter { get; set; }

        private NamedTypeDictionary _stemTypeDictionary;

        private readonly CollectionCreatorCache _creators = new CollectionCreatorCache();

        public IEnumerable<Type> GetStemTypes()
        {
            if (DataSource == null) throw new StemStartSetupException("DataSource must be provided.");
            if (StemTypeGetter == null) throw new StemStartSetupException("StemTypeLocaator must be provided.");

            _stemTypeDictionary = new AttributedSuffixedDerivedTypeDictionary(typeof(Stem), "Stem", typeof(DataSourceRootAttribute));
            _stemTypeDictionary.AddVaidTypes(StemTypeGetter);
            return _stemTypeDictionary.GetAllTypes();
        }

        public IRestResource GetStartResource(IRootRequest request, IStemConfiguration configuration)
        {
            return new DataSourceDirectory(configuration, request, _stemTypeDictionary, _creators, DataSource);
        }
    }
}