using System.Collections.Generic;
using Common.Equipments;
using Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common
{
    public class Inventory : MonoBehaviour, ISavable
    {
        [SerializeField] private EquipType equipType;
        
        [ShowInInspector]
        private List<Equipment> List { get; } = new();
        private string SaveKey => $"{equipType}Inventory";

        public void Add(Equipment item)
        {
            if(item.EquipType != equipType) return;
            
            item.gameObject.transform.SetParent(transform);
            List.Add(item);
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
            
            if (infoList.HasElement()) 
                infoList.ForEach(Generate);
        }
        

        private void Generate(EquipmentInfo info)
        {
            if (info.ActionCode == DataIndex.None) return;
            
            Database.EquipmentMaster.GetObject(info.ActionCode, out var equipmentPrefab)
                    .OnFalse(() => Debug.LogWarning($"Not Exist {info.ActionCode} prefab"));

            var equipObject = Instantiate(equipmentPrefab, transform);
        
            if (!equipObject.TryGetComponent(out Equipment equipment))
            {
                Debug.LogWarning($"Not Exist Equipment script in {equipmentPrefab.name} GameObject");
                return;
            }
        
            equipment.Enchant(info.EnchantLevel);
            
            Add(equipment);
        }
    }
}
