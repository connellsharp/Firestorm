using System.Collections.Generic;
using System.Linq;
using Firestorm.Data;
using Firestorm.Fluent.Fuel.Sources;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Models
{
    internal class ApiItemModel<TItem> : IApiItemModel
        where TItem : class, new()
    {
        private readonly IDataSource _dataSource;

        public ApiItemModel(IDataSource dataSource)
        {

            _dataSource = dataSource;
        }

        internal IList<ApiFieldModel<TItem>> Fields { get; } = new List<ApiFieldModel<TItem>>();

        internal IList<ApiIdentifierModel<TItem>> Identifiers { get; } = new List<ApiIdentifierModel<TItem>>();

        public string RootName { get; set; }

        public IRestCollectionSource GetRootCollectionSource()
        {
            return new RestCollectionSource<TItem>(_dataSource, Fields.ToDictionary(f => f.Name), Identifiers.ToDictionary(i => i.Name));
        }
    }
}