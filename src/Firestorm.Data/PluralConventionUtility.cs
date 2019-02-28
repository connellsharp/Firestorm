using System;
using System.Collections.Generic;

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

            string singularLower = singular.ToLower();
            if (SpecialCases.ContainsKey(singularLower))
            {
                return SpecialCases[singularLower];
            }

            foreach (var replacement in EndingReplacements)
            {
                if (singular.EndsWith(replacement.Key, comp))
                {
                    return singular.TrimEnd(replacement.Key) + replacement.Value;
                }
            }

            return singular + "s";
        }

        private static readonly IDictionary<string, string> SpecialCases = new Dictionary<string, string>
        {
            {"person", "People"},
            {"tooth", "Teeth"},
        };

        private static readonly IDictionary<string, string> EndingReplacements = new Dictionary<string, string>
        {
            {"y", "ies"},
            {"f", "ves"},
            {"fe", "ves"},
            {"s", "ses"},
            {"ch", "ches"},
            {"x", "xes"},
            {"z", "zes"},
            {"sh", "shes"},
        };
    }
}