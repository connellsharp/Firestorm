using System.Collections.Concurrent;

namespace Firestorm.Stems.Roots.Derive
{
    internal class CollectionCreatorCache : ConcurrentDictionary<string, IRootCollectionCreator>
    { }
}