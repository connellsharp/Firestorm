using System.Collections.Generic;

namespace Firestorm.Core.Web
{
    public class CollectionBody : ResourceBody, IPagedResourceBody
    {
        public CollectionBody(RestCollectionData collection, PageLinks pageLinks)
        {
            Items = collection.Items;
            PageLinks = pageLinks;
        }

        public IEnumerable<RestItemData> Items { get; }

        public PageLinks PageLinks { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Collection;

        public override object GetObject() => Items;
    }
}