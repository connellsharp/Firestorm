using System.Collections.Generic;
using System.Linq;
using Firestorm.Fluent.Fuel.Sources;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Models
{
    internal class ApiItemModel<TItem> : IApiItemModel
        where TItem : class
    {
        internal IList<ApiFieldModel<TItem>> Fields { get; } = new List<ApiFieldModel<TItem>>();

        internal IList<ApiIdentifierModel<TItem>> Identifiers { get; } = new List<ApiIdentifierModel<TItem>>();

        public string RootName { get; set; }

        public IRestCollectionSource GetCollectionSource()
        {
            return new RestCollectionSource<TItem>(Fields.ToDictionary(f => f.Name));
        }
    }
}