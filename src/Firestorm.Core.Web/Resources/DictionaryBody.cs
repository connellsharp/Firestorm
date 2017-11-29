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
            //PageLinks = dictionary.PageDetails; TODO
        }

        public RestItemData Items { get; }

        public PageLinks PageLinks { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Dictionary;

        public override object GetObject() => Items;
    }
}