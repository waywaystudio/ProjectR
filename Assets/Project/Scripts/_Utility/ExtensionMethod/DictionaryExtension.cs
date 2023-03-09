using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;

namespace Core
{
    public static class DictionaryExtension
    {
        public static Dictionary<TKey, TValue> Ref<TKey, TValue>(this Dictionary<TKey, TValue> table, List<TValue> list, Func<TValue, TKey> keySelector)
        {
            if (table.IsNullOrEmpty())
            {
                table = list.ToDictionary(keySelector);
            }

            return table;
        } 
        
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

        public static Dictionary<TKey, List<TValue>> Combine<TKey, TValue>(IEqualityComparer<TKey> comparer = null, params IDictionary<TKey, TValue>[] dictionaries)
        {
            comparer ??= EqualityComparer<TKey>.Default;
            var result = new Dictionary<TKey, List<TValue>>(comparer);
            
            var allKeys = dictionaries.SelectMany(dict => dict.Keys).Distinct(comparer);
            
            foreach (var key in allKeys)
            {
                var list = new List<TValue>();
                foreach (var dict in dictionaries)
                {
                    if (dict.TryGetValue(key, out var value)) list.Add(value);
                }
                result.Add(key, list);
            }

            return result;
        }
    }
}
