using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace Common.PartyCamps.Inventories
{
    public class MaterialInventory : MonoBehaviour, ISavable
    {
        [SerializeField] private DataIndex category;
        
        private const string MaterialSerializeKey = "MaterialSerializeKey";

        public DataIndex Category => category;
        public Dictionary<MaterialType, int> MaterialTable { get; set; } = new();
        
        public void Add(MaterialType type, int count)
        {
            if (MaterialTable.ContainsKey(type))
            {
                MaterialTable[type] += count;
            }
            else
            {
                MaterialTable.Add(type, count);
            }
        }
        
        public void Consume(MaterialType type, int count)
        {
            if (!IsEnough(type, count)) return;
            
            MaterialTable[type] -= count;
        }

        public int GetCount(MaterialType type) =>  MaterialTable.TryGetValue(type, out var value) 
            ? value 
            : 0;
        
        public void Save()
        {
            SaveManager.Save(MaterialSerializeKey, MaterialTable);
        }
        
        public void Load()
        {
            MaterialTable = SaveManager.Load(MaterialSerializeKey, new Dictionary<MaterialType, int>());
        }
        

        private bool IsEnough(MaterialType type, int count)
        {
            if (!MaterialTable.ContainsKey(type)) return false;
            return MaterialTable[type] >= count;
        }
    }
}
