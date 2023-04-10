using System;
using Common.Equipments;
using Common.PlayerCamps.Inventories;
using UnityEngine;

namespace Common.PlayerCamps
{
    public class InventoryManager : MonoBehaviour, IEditable
    {
        [SerializeField] private Inventory weaponInventory;
        [SerializeField] private Inventory headInventor;
        [SerializeField] private Inventory topInventory;
        [SerializeField] private Inventory bottomInventory;
        [SerializeField] private Inventory trinketInventory;


        public void Add(GameObject itemObject) => Add(itemObject.GetComponent<Equipment>());
        public void Add(Equipment item)
        {
            var targetInventory = GetInventoryByType(item.EquipType);
            
            targetInventory.Add(item);
        }

        public void Remove(GameObject itemObject) => Remove(itemObject.GetComponent<Equipment>());
        public void Remove(Equipment item)
        {
            var targetInventory = GetInventoryByType(item.EquipType);
            
            targetInventory.Remove(item);
        }


        private Inventory GetInventoryByType(EquipType type) => type switch
        {
            EquipType.Weapon  => weaponInventory,
            EquipType.Head    => headInventor,
            EquipType.Top     => topInventory,
            EquipType.Bottom  => bottomInventory,
            EquipType.Trinket => trinketInventory,
            _                 => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            weaponInventory  = transform.Find("Weapon").GetComponent<Inventory>();
            headInventor     = transform.Find("Head").GetComponent<Inventory>();
            topInventory     = transform.Find("Top").GetComponent<Inventory>();
            bottomInventory  = transform.Find("Bottom").GetComponent<Inventory>();
            trinketInventory = transform.Find("Trinket").GetComponent<Inventory>();
        }
#endif
    }
}
