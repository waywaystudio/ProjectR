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

        /// <summary>
        /// TryAdd Extended overwrite Option. Return always true (just for match original function's return type)
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> table, TKey key, TValue value, bool overwrite)
        {
            if (!table.TryAdd(key, value) && overwrite)
            {
                table[key] = value;
            }

            return true;
        }
        
        public static double Sum<TKey>(this Dictionary<TKey, double> table)
        {
            var result = 0d;
            
            table.ForEach(x => result += x.Value);

            return result;
        }
    }
}
