using System.Collections.Generic;

namespace Firestorm
{
    /// <summary>
    /// Core class containing items and information about a dictionary used in a Firestorm API.
    /// </summary>
    public class RestDictionaryData
    {
        public RestDictionaryData(IEnumerable<KeyValuePair<string, object>> items)
        {
            Items = new RestItemData(items);
        }

        public RestItemData Items { get; }

        public PageDetails PageDetails { get; }
    }
}