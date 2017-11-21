using System.Collections.Generic;
using Firestorm.Fluent.Fuel.Sources;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Models
{
    internal class ApiItemModel<TItem> : IApiItemModel
        where TItem : class
    {
        internal IDictionary<string, ApiFieldModel<TItem>> Fields { get; } = new Dictionary<string, ApiFieldModel<TItem>>();

        internal IDictionary<string, ApiIdentifierModel<TItem>> Identifiers { get; } = new Dictionary<string, ApiIdentifierModel<TItem>>();

        public string RootName { get; set; }

        public IRestCollectionSource GetCollectionSource()
        {
            return new RestCollectionSource<TItem>(Fields);
        }
    }
}