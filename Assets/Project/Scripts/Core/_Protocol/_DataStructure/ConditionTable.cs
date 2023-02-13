using System;
using UnityEngine;

namespace Core
{
    public class ConditionTable : DelegateTable<string, Func<bool>>
    {
        public void Register(string key, Func<bool> predicate)
        {
            if (ContainsKey(key)) Debug.LogWarning($"Key is already Exist. key:{key}");

            TryAdd(key, predicate);
        }
        
        public bool IsAllTrue
        { 
            get
            {
                foreach (var item in this) 
                    if (!item.Value.Invoke()) return false;

                return true;
            }
        }
        public bool IsAllFalse
        {
            get
            {
                foreach (var item in this) 
                    if (item.Value.Invoke()) return false;

                return true;
            }
        }
        public bool HasTrue
        {
            get
            {
                foreach (var item in this) 
                    if (item.Value.Invoke()) return true;

                return false;
            }
        }
        public bool HasFalse
        {
            get
            {
                foreach (var item in this) 
                    if (!item.Value.Invoke()) return true;

                return false;
            }
        }
        public int TrueCount
        {
            get
            {
                var result = 0;

                foreach (var item in this)
                {
                    if (item.Value.Invoke()) result++;
                }

                return result;
            }
        }
        public int FalseCount
        {
            get
            {
                var result = 0;

                foreach (var item in this)
                {
                    if (!item.Value.Invoke()) result++;
                }

                return result;
            }
        }
    }
}
