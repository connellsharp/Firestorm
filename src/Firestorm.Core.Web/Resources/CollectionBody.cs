using System.Collections.Generic;

namespace Firestorm.Core.Web
{
    public class CollectionBody : ResourceBody, IPagedResourceBody
    {
        public CollectionBody(IEnumerable<RestItemData> items, PageLinks pageLinks)
        {
            PageLinks = pageLinks;
            Items = items;
        }

        public IEnumerable<RestItemData> Items { get; }

        public PageLinks PageLinks { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Collection;

        public override object GetObject() => Items;
    }
}