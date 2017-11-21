using System.Collections.Generic;
using Firestorm.Engine.Identifiers;
using Firestorm.Fluent.Fuel.Sources;

namespace Firestorm.Fluent.Fuel.Definitions
{
    public class EngineApiModel
    {
        public IDictionary<string, IApiItemModel> Items { get; } = new Dictionary<string, IApiItemModel>();
    }

    public class ApiItemModel<TItem> : IApiItemModel
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

    internal class ApiIdentifierModel<TItem>
    {
        public IIdentifierInfo<TItem> IdentifierInfo { get; set; }

        public string Name { get; set; }
    }

    public interface IApiItemModel
    {
        IRestCollectionSource GetCollectionSource();
    }
}