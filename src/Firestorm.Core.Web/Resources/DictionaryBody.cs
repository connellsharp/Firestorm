using System.Collections.Generic;

namespace Firestorm.Core.Web
{
    /// <remarks>
    /// Similar to <see cref="CollectionBody"/>
    /// </remarks>
    public class DictionaryBody : ResourceBody
    {
        public DictionaryBody(RestDictionaryData dictionary)
        {
            Items = dictionary.Items;
        }

        public DictionaryBody(IEnumerable<KeyValuePair<string, object>> items)
            : this(new RestDictionaryData(items))
        { }

        public RestItemData Items { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Dictionary;

        public override object GetObject() => Items;
    }
}