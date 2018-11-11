using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Firestorm
{
    public class DynamicObjectImporter : IRestItemDataImporter
    {
        public bool CanImport(object obj)
        {
            return obj is IDynamicMetaObjectProvider;
        }

        public Type GetType(object obj)
        {
            return obj.GetType();
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues(object obj)
        {
            var dynamicType = (IDynamicMetaObjectProvider)obj;
            throw new NotImplementedException();
        }
    }
}