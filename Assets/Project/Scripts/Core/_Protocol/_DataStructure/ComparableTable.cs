using System.Collections.Generic;

namespace Core
{
    public abstract class ComparableTable<TKey, TValue> : Dictionary<TKey, TValue>, IComparer<TValue>
    {
        public void Register(TKey key, TValue value)
        {
            if (!ContainsKey(key)) 
                Add(key, value);
            else if (Compare(this[key], value) > 0) 
                this[key] = value;
        }

        public void Unregister(TKey key) => this.TryRemove(key);

        /// <returns>1 == True, 0 == Same, -1 == False</returns>
        public abstract int Compare(TValue x, TValue y);
    }
}
