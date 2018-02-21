using System.Collections.Concurrent;

namespace Firestorm.Stems.Roots.Combined
{
    internal class CollectionCreatorCache : ConcurrentDictionary<string, IDataSourceCollectionCreator>
    { }
}