using System.Collections.Generic;
using Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Camps.Inventories
{
    public class ViceMaterialInventory : MonoBehaviour, ISavable
    {
        private const string MaterialSerializeKey = "ViceMaterialSerializeKey";
        
        [ShowInInspector]
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
        
        
#if UNITY_EDITOR
        [Button(ButtonSizes.Large, ButtonStyle.Box)]
        public void AddAll100Material()
        {
            ViceMaterialType.None.Iterator(viceType =>
            {
                if (viceType == ViceMaterialType.None) return;
                
                Add(viceType, 100);
            });
        }
#endif
    }
}
