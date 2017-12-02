using System;
using System.Linq;
using System.Reflection;

namespace Firestorm.Data
{
    public class PluralConventionPrimaryKeyFinder : IPrimaryKeyFinder
    {
        public PropertyInfo GetPrimaryKeyInfo(Type type)
        {
            throw new NotImplementedException("Not implemented PluralConventionPrimaryKeyFinder yet.");
            //type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.Name ==);
        }
    }
}