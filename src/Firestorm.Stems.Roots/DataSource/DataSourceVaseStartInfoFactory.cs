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
        private readonly DataSourceRootAttributeBehavior _rootBehavior;

        private NamedTypeDictionary _stemTypeDictionary;

        public DataSourceVaseStartInfoFactory(IDataSource dataSource, ITypeGetter stemTypeGetter, DataSourceRootAttributeBehavior rootBehavior)
        {
            _dataSource = dataSource;
            _stemTypeGetter = stemTypeGetter;
            _rootBehavior = rootBehavior;
        }

        public IEnumerable<Type> GetStemTypes(IStemsCoreServices configuration)
        {
            _stemTypeDictionary = new SuffixedNamedTypeDictionary("Stem");

            var validator = new AggregateTypeValidator(
                GetAttributedTypeValidator(),
                new DerivedTypeValidator(typeof(Stem))
            );

            var populator = new TypeDictionaryPopulator(_stemTypeDictionary, validator);
            populator.AddValidTypes(_stemTypeGetter);

            return _stemTypeDictionary.GetAllTypes();
        }

        public IRootStartInfo Get(IStemsCoreServices configuration, string startResourceName)
        {
            return new DataSourceVaseStartInfo(_dataSource, _stemTypeDictionary, startResourceName);
        }

        public RestDirectoryInfo CreateDirectoryInfo()
        {
            return _stemTypeDictionary.CreateDirectoryInfo();
        }

        private AttributedTypeValidator GetAttributedTypeValidator()
        {
            switch (_rootBehavior)
            {
                case DataSourceRootAttributeBehavior.OnlyUseAllowedStems:
                    return new AttributedTypeValidator(typeof(DataSourceRootAttribute), true);

                case DataSourceRootAttributeBehavior.UseAllStemsExceptDisallowed:
                    return new AttributedTypeValidator(typeof(NoDataSourceRootAttribute), false);

                default:
                    throw new ArgumentOutOfRangeException(nameof(DataSourceRootAttributeBehavior), "DataSourceRootAttributeBehavior was invalid.");
            }
        }
    }
}