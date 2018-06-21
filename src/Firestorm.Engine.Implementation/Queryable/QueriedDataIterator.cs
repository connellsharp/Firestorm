using System.Collections;
using System.Collections.Generic;

namespace Firestorm.Engine.Queryable
{
    /// <summary>
    /// Iterates round a List of anonymous objects and converts to <see cref="RestItemData"/> objects.
    /// Also provides a couple of extra bits to manipulate the underlying list.
    /// </summary>
    internal class QueriedDataIterator : IEnumerable<RestItemData>
    {
        private readonly List<object> _queriedDynamicObjects;

        public QueriedDataIterator(List<object> queriedDynamicObjects)
        {
            _queriedDynamicObjects = queriedDynamicObjects;
        }

        public int Length => _queriedDynamicObjects.Count;

        public IEnumerator<RestItemData> GetEnumerator()
        {
            foreach (object obj in _queriedDynamicObjects)
            {
                yield return obj == null ? null : new RestItemData(obj);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void RemoveLastItem()
        {
            _queriedDynamicObjects.RemoveAt(_queriedDynamicObjects.Count - 1);
        }
    }
}