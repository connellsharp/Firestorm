using System;
using System.Collections.Generic;
using Firestorm.Data;
using JetBrains.Annotations;

namespace Firestorm.Extensions.AspNetCore
{
    internal class DataSourceTypeFinder : IItemTypeFinder
    {
        private readonly IDiscoverableDataSource _discoverableDataSource;

        public DataSourceTypeFinder([NotNull] IDataSource dataSource)
        {
            if (dataSource is IDiscoverableDataSource searchableDataSource)
            {
                _discoverableDataSource = searchableDataSource;
            }
            else
            {
                throw new ArgumentException("Data source must be searachable to automatically find the item types.", nameof(dataSource));
            }
        }

        public IEnumerable<Type> FindItemTypes()
        {
            return _discoverableDataSource.FindRespositoryTypes();
        }
    }
}