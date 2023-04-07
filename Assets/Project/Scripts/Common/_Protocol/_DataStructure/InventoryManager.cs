using System;
using Common.Equipments;
using Singleton;
using UnityEngine;

namespace Common
{
    public class InventoryManager : MonoSingleton<InventoryManager>, IEditable
    {
        [SerializeField] private Inventory weaponInventory;
        [SerializeField] private Inventory headInventor;
        [SerializeField] private Inventory topInventory;
        [SerializeField] private Inventory bottomInventory;
        [SerializeField] private Inventory trinketInventory;


        public static void Add(GameObject itemObject) => Add(itemObject.GetComponent<Equipment>());
        public static void Add(Equipment item)
        {
            var targetInventory = GetInventoryByType(item.EquipType);
            
            targetInventory.Add(item);
        }

        public static void Remove(GameObject itemObject) => Remove(itemObject.GetComponent<Equipment>());
        public static void Remove(Equipment item)
        {
            var targetInventory = GetInventoryByType(item.EquipType);
            
            targetInventory.Remove(item);
        }


        private static Inventory GetInventoryByType(EquipType type) => type switch
        {
            EquipType.Weapon  => Instance.weaponInventory,
            EquipType.Head    => Instance.headInventor,
            EquipType.Top     => Instance.topInventory,
            EquipType.Bottom  => Instance.bottomInventory,
            EquipType.Trinket => Instance.trinketInventory,
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
