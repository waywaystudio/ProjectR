using System.Collections.Generic;
using Serialization;

namespace Camps
{
    public abstract class Inventory<T> : ISavable
    {
        protected abstract string SerializeKey { get; }
        protected Dictionary<T, int> Table { get; set; } = new();
        

        public void Add(T type, int count)
        {
            if (Table.ContainsKey(type))
            {
                Table[type] += count;
            }
            else
            {
                Table.Add(type, count);
            }
        }
        
        public void Consume(T type, int count)
        {
            if (!IsEnough(type, count)) return;
            
            Table[type] -= count;
        }
        
        public int Count(T type) => Table.TryGetValue(type, out var value) 
            ? value 
            : 0;
        
        public void Save()
        {
            SaveManager.Save(SerializeKey, Table);
        }
        
        public void Load()
        {
            Table = SaveManager.Load(SerializeKey, new Dictionary<T, int>());
        }
        
        
        private bool IsEnough(T type, int count)
        {
            if (!Table.ContainsKey(type)) return false;
            return Table[type] >= count;
        }
    }
}
