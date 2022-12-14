using System.Collections.Generic;

namespace Core
{
    public static class DictionaryExtension
    {
        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> table, TKey key)
        {
            if (!table.ContainsKey(key)) return false;
            
            table.Remove(key);
            return true;

        }
    }
}
