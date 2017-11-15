using System.Collections.Concurrent;

namespace Firestorm.Stems.Roots.DataSource
{
    internal class CollectionCreatorCache : ConcurrentDictionary<string, IDataSourceCollectionCreator>
    { }
}