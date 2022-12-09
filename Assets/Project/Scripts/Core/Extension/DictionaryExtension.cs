using System.Collections.Generic;

namespace Core
{
    public static class DictionaryExtension
    {
        public static void RemoveSafely<TKey, TValue>(this Dictionary<TKey, TValue> table, TKey key)
        {
            if (table.ContainsKey(key))
                table.Remove(key);
        }
    }
}
