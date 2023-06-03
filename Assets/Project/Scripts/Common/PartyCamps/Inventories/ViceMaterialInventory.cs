using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace Common.PartyCamps.Inventories
{
    public class ViceMaterialInventory : MonoBehaviour, ISavable
    {
        [SerializeField] private DataIndex category;
        
        private const string MaterialSerializeKey = "MaterialSerializeKey";

        public DataIndex Category => category;
        
        [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<ViceMaterialType, int> MaterialTable { get; set; } = new();
        
        public void Add(ViceMaterialType type, int count)
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
        
        public void Consume(ViceMaterialType type, int count)
        {
            if (!IsEnough(type, count)) return;
            
            MaterialTable[type] -= count;
        }

        public int GetCount(ViceMaterialType type) =>  MaterialTable.TryGetValue(type, out var value) 
            ? value 
            : 0;
        
        public void Save()
        {
            SaveManager.Save(MaterialSerializeKey, MaterialTable);
        }
        
        public void Load()
        {
            MaterialTable = SaveManager.Load(MaterialSerializeKey, new Dictionary<ViceMaterialType, int>());
        }
        

        private bool IsEnough(ViceMaterialType type, int count)
        {
            if (!MaterialTable.ContainsKey(type)) return false;
            return MaterialTable[type] >= count;
        }
    }
}
