using System.Collections.Generic;

namespace Firestorm.Stems.Attributes.Definitions
{
    public static class DictionaryExtensions
    {
        // from http://stackoverflow.com/a/16193323/369247
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            TValue val;

            if (!dict.TryGetValue(key, out val))
            {
                val = new TValue();
                dict.Add(key, val);
            }

            return val;
        }
    }
}