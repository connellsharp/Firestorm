using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// Tries to identify the primary key property using common naming conventions.
    /// </summary>
    public class IDConventionIdentifierInfo<TItem> : PropertyInfoIdentifierInfo<TItem>
    {
        public IDConventionIdentifierInfo()
            : base(GetKeyPropertyByName())
        {
        }

        public static PropertyInfo GetKeyPropertyByName()
        {
            PropertyInfo[] properties = typeof(TItem).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string typeName = typeof(TItem).Name;
                var possibleNames = new[] { "ID", typeName + "ID", typeName + "_ID" };

                if (possibleNames.Any(name => string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase)))
                    return property;
            }

            throw new KeyNotFoundException("Identifier could not be determined for item type " + typeof(TItem).Name);
        }
    }
}