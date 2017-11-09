using System.Collections.Generic;

namespace Firestorm.Core.Web
{
    public class CollectionBody : ResourceBody, IPagedResourceBody
    {
        public CollectionBody(RestCollectionData collection)
        {
            Items = collection.Items;
            PageDetails = collection.PageDetails;
        }

        public IEnumerable<RestItemData> Items { get; }

        public PageDetails PageDetails { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Collection;

        public override object GetObject() => Items;
    }
}