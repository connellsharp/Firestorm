using System;
using System.Collections.Generic;

namespace Firestorm.Stems.Definitions
{   
    public class NamedDictionary<T> : Dictionary<string, T>
    {
        private readonly Func<string, T> _createFromName;

        internal NamedDictionary(Func<string, T> createFromName)
        {
            _createFromName = createFromName;
        }
                
        public T GetOrCreate(string name)
        {
            if (!TryGetValue(name, out T obj))
            {
                obj = _createFromName(name);
                Add(name, obj);
            }

            return obj;
        }
    }
}