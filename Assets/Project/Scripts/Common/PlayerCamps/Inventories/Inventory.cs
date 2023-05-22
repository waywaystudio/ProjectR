using System.Collections.Generic;
using Common.Equipments;
using Serialization;
using UnityEngine;

namespace Common.PlayerCamps.Inventories
{
    public class Inventory : MonoBehaviour, ISavable
    {
        [SerializeField] private EquipType equipType;

        public Dictionary<MaterialType, int> MaterialInventory { get; set; } = new();

        public void AddMaterial(MaterialType type, int count)
        {
            if (MaterialInventory.ContainsKey(type))
            {
                MaterialInventory[type] += count;
            }
            else
            {
                MaterialInventory.Add(type, count);
            }
        }
        
        public void ConsumeMaterial(MaterialType type, int count)
        {
            if (!IsEnough(type, count)) return;
            
            MaterialInventory[type] -= count;
        }

        public bool IsEnough(MaterialType type, int count)
        {
            if (!MaterialInventory.ContainsKey(type)) return false;
            return MaterialInventory[type] >= count;
        }

        

        // private List<Equipment> List { get; } = new();
        // private string SaveKey => $"{equipType}Inventory";

        // public List<Equipment> GetList() => List;

        // public void Add(Equipment item)
        // {
            // if(item.IsNullOrEmpty() || item.EquipType != equipType) return;
            //
            // item.gameObject.transform.SetParent(transform);
            // List.AddUniquely(item);
        // }

        // public void Remove(Equipment item)
        // {
            // List.RemoveSafely(item);
        // }

        public void Save()
        {
            // var infoList = new List<EquipmentInfo>(List.Count);
            // List.ForEach(element => infoList.Add(element.Info));
            // SaveManager.Save(SaveKey, infoList);
        }
        
        public void Load()
        {
            // List.ForReverse(element =>
            // {
            //     Remove(element);
            //     Destroy(element.gameObject);
            // });
            //
            // var infoList = SaveManager.Load<List<EquipmentInfo>>(SaveKey);
            //
            // if (infoList.IsNullOrEmpty()) return;
            //
            // infoList.ForEach(equipInfo =>
            // {
            //     var equipment = EquipmentInfo.CreateEquipment(equipInfo, transform);
            //
            //     Add(equipment);
            // });
        }
    }
}
