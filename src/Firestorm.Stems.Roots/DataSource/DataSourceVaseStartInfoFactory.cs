using System;
using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Stems.Roots.Combined;

namespace Firestorm.Stems.Roots.DataSource
{
    public class DataSourceVaseStartInfoFactory : IRootStartInfoFactory
    {
        private readonly IDataSource _dataSource;
        private readonly ITypeGetter _stemTypeGetter;
        private NamedTypeDictionary _stemTypeDictionary;

        public DataSourceVaseStartInfoFactory(IDataSource dataSource, ITypeGetter stemTypeGetter)
        {
            _dataSource = dataSource;
            _stemTypeGetter = stemTypeGetter;
        }

        public IEnumerable<Type> GetStemTypes()
        {
            _stemTypeDictionary = new AttributedSuffixedDerivedTypeDictionary(typeof(Stem), "Stem", typeof(DataSourceRootAttribute));
            _stemTypeDictionary.AddVaidTypes(_stemTypeGetter);

            return _stemTypeDictionary.GetAllTypes();
        }

        public IRootStartInfo Get(IStemConfiguration configuration, string startResourceName)
        {
            return new DataSourceVaseStartInfo(_dataSource, _stemTypeDictionary, startResourceName);
        }

        public RestDirectoryInfo CreateDirectoryInfo()
        {
            return _stemTypeDictionary.CreateDirectoryInfo();
        }
    }
}