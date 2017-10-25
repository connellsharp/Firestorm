using System;
using System.Collections.Generic;

namespace Firestorm
{
    public class DictionaryImporter : IRestItemDataImporter
    {
        public bool CanImport(object obj)
        {
            return obj is IEnumerable<KeyValuePair<string, object>>;
        }

        public Type GetType(object obj)
        {
            return obj.GetType();
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues(object obj)
        {
            return (IEnumerable<KeyValuePair<string, object>>)obj;
        }
    }
}