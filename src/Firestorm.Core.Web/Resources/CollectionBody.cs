using System.Collections.Generic;

namespace Firestorm.Core.Web
{
    public class CollectionBody : ResourceBody
    {
        public CollectionBody(RestCollectionData collection)
        {
            Items = collection.Items;
        }

        public CollectionBody(IEnumerable<RestItemData> items)
            : this(new RestCollectionData(items))
        { }

        public IEnumerable<RestItemData> Items { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Collection;

        public override object GetObject() => Items;
    }
}