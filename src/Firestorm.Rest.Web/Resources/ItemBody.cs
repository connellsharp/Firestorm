namespace Firestorm.Rest.Web
{
    public class ItemBody : ResourceBody
    {
        public ItemBody(RestItemData item)
        {
            Item = item;
        }

        public RestItemData Item { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Item;

        public override object GetObject() => Item;
    }
}