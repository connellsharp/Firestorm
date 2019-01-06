namespace Firestorm.Rest.Web
{
    /// <remarks>
    /// Similar to <see cref="CollectionBody"/>
    /// </remarks>
    public class DictionaryBody : ResourceBody, IPagedResourceBody
    {
        public DictionaryBody(RestItemData items, PageLinks pageLinks)
        {
            Items = items;
            PageLinks = pageLinks;
        }

        public RestItemData Items { get; }

        public PageLinks PageLinks { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Dictionary;

        public override object GetObject() => Items;
    }
}