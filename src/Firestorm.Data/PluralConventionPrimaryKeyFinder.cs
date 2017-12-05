using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Firestorm.Data
{
    public class PluralConventionPrimaryKeyFinder : IPrimaryKeyFinder
    {
        public PropertyInfo GetPrimaryKeyInfo(Type type)
        {
            var possibleNames = new HashSet<string> { "ID", "Id", type.Name + "ID", type.Name + "Id" };

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => possibleNames.Contains(p.Name)).ToList();

            if (properties.Count == 0)
                throw new KeyNotFoundException("There were no ID property names in the " + type.Name + " class");

            if (properties.Count > 1)
                throw new AmbiguousMatchException("Several possible ID properties in the " + type.Name + " class");

            return properties[0];
        }
    }
}