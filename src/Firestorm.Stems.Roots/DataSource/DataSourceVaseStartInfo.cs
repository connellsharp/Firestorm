using System;
using Firestorm.Data;
using Firestorm.Stems.Roots.Combined;

namespace Firestorm.Stems.Roots.DataSource
{
    public class DataSourceVaseStartInfo : IRootStartInfo
    {
        private readonly NamedTypeDictionary _stemTypeDictionary;
        private readonly string _startResourceName;
        private readonly IDataSource _dataSource;

        public DataSourceVaseStartInfo(IDataSource dataSource, NamedTypeDictionary stemTypeDictionary, string startResourceName)
        {
            _stemTypeDictionary = stemTypeDictionary;
            _startResourceName = startResourceName;
            _dataSource = dataSource;
        }

        public Type GetStemType()
        {
            return _stemTypeDictionary.GetType(_startResourceName);
        }

        public IAxis GetAxis(IStemsCoreServices services, IRestUser user)
        {
            return new Vase
            {
                Services = services,
                User = user
            };
        }

        public IDataSource GetDataSource()
        {
            return _dataSource;
        }
    }
}