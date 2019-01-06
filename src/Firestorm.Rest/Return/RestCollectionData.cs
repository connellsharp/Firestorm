using System.Collections.Generic;

namespace Firestorm
{
    /// <summary>
    /// Core class containing items and information about a collection used in a Firestorm API.
    /// </summary>
    public class RestCollectionData
    {
        public RestCollectionData(IEnumerable<RestItemData> items, PageDetails pageDetails)
        {
            Items = items;
            PageDetails = pageDetails;
        }

        public IEnumerable<RestItemData> Items { get; }

        public PageDetails PageDetails { get; }
    }
}