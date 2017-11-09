namespace Firestorm.Core.Web
{
    /// <remarks>
    /// Similar to <see cref="CollectionBody"/>
    /// </remarks>
    public class DictionaryBody : ResourceBody, IPagedResourceBody
    {
        public DictionaryBody(RestDictionaryData dictionary)
        {
            Items = dictionary.Items;
            PageDetails = dictionary.PageDetails;
        }

        public RestItemData Items { get; }

        public PageDetails PageDetails { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Dictionary;

        public override object GetObject() => Items;
    }
}