using System.Collections.Generic;
using Firestorm.Data;

namespace Firestorm.Fluent.Fuel.Models
{
    public class EngineApiModel
    {
        public EngineApiModel(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        internal IDataSource DataSource { get; }

        public IList<IApiItemModel> Items { get; } = new List<IApiItemModel>();
    }
}