using System;
using System.Collections.Generic;

namespace Firestorm
{
    public class RestItemDataImporter : IRestItemDataImporter
    {
        public bool CanImport(object obj)
        {
            return obj is RestItemData;
        }

        public Type GetType(object obj)
        {
            return ((RestItemData)obj).Type;
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues(object obj)
        {
            return (RestItemData) obj;
        }
    }
}