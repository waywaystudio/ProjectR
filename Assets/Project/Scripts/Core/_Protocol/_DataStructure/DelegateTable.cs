using System;
using System.Collections.Generic;

namespace Core
{
    public class DelegateTable<TKey, TValue> : Dictionary<TKey, TValue> where TValue : Delegate
    {
        protected DelegateTable(){}
        protected DelegateTable(int capacity) : base(capacity) {}

        public void Unregister(TKey key) => this.TryRemove(key);
        public void UnregisterAll() => Clear();

        protected void TryAdd(TKey key, TValue value, bool overwrite)
        {
            if (!TryAdd(key, value) && overwrite) this[key] = value;
        }
    }
}
