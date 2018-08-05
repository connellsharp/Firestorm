using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Firestorm.Data;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// Tries to identify the primary key property using common naming conventions.
    /// </summary>
    public class IdConventionIdentifierInfo<TItem> : PropertyInfoIdentifierInfo<TItem>
    {
        public IdConventionIdentifierInfo()
            : base(GetKeyPropertyByName())
        {
        }

        public static PropertyInfo GetKeyPropertyByName()
        {
            string typeName = typeof(TItem).Name;
            var possibleNames = IdConventionPrimaryKeyFinder.GetPossibleIdNames(typeName);

            PropertyInfo[] properties = typeof(TItem).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (possibleNames.Contains(property.Name))
                    return property;
            }

            throw new KeyNotFoundException("Identifier could not be determined for item type " + typeof(TItem).Name);
        }
    }
}