using System;

namespace Firestorm.Data
{
    public static class PluralConventionUtility
    {
        public static string GetTableName<T>()
        {
            return GetTableName(typeof(T));
        }

        public static string GetTableName(Type entityType)
        {
            return Pluralize(entityType.Name);
        }

        public static string Pluralize(string singular)
        {
            const StringComparison comp = StringComparison.InvariantCultureIgnoreCase;

            if (singular.ToLower() == "person")
                return "People";

            if (singular.ToLower() == "tooth")
                return "Teeth";

            if (singular.EndsWith("y", comp))
                return singular.Remove(singular.Length - 1, 1) + "ies";

            if (singular.EndsWith("f", comp))
                return singular.Remove(singular.Length - 1, 1) + "ves";

            if (singular.EndsWith("fe", comp))
                return singular.Remove(singular.Length - 2, 2) + "ves";

            if (singular.EndsWith("s", comp) || singular.EndsWith("ch", comp) || singular.EndsWith("x", comp) || singular.EndsWith("z", comp) || singular.EndsWith("sh", comp))
                return singular + "es";

            return singular + "s";
        }
    }
}