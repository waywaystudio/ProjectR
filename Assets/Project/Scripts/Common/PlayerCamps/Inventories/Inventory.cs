using System.Collections.Generic;
using Common.Equipments;
using Serialization;
using UnityEngine;

namespace Common.PlayerCamps.Inventories
{
    public class Inventory : MonoBehaviour, ISavable
    {
        [SerializeField] private EquipType equipType;
        
        [Sirenix.OdinInspector.ShowInInspector]
        private List<Equipment> List { get; } = new();
        private string SaveKey => $"{equipType}Inventory";

        public void Add(Equipment item)
        {
            if(item.IsNullOrEmpty() || item.EquipType != equipType) return;
            
            item.gameObject.transform.SetParent(transform);
            List.AddUniquely(item);
        }

        public void Remove(Equipment item)
        {
            List.RemoveSafely(item);
            
            Destroy(item);
        }

        public void Save()
        {
            var infoList = new List<EquipmentInfo>(List.Count);
            
            List.ForEach(element => infoList.Add(element.Info));
        
            SaveManager.Save(SaveKey, infoList);
        }
        
        public void Load()
        {
            List.ForEach(Remove);
            
            var infoList = SaveManager.Load<List<EquipmentInfo>>(SaveKey);
        
            if (infoList.IsNullOrEmpty()) return;
            
            infoList.ForEach(equipInfo =>
            {
                var equipment = EquipmentInfo.CreateEquipment(equipInfo, transform);
        
                Add(equipment);
            });
        }
    }
}
