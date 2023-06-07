using System.Collections.Generic;
using Serialization;
using Singleton;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Common
{
    using Camps.Inventories;
    using Characters;
    
    public class Camp : MonoSingleton<Camp>, ISavable, IEditable
    {
        [SerializeField] private List<VenturerData> characterDataList;

        private readonly GrowMaterialInventory growMaterialInventory = new();  

        private static List<VenturerData> CharacterDataList => Instance.characterDataList;
        private static GrowMaterialInventory GrowMaterialInventory => Instance.growMaterialInventory;

        /*
         * Venturer
         */
        public static VenturerData GetData(VenturerType type) => CharacterDataList.TryGetElement(data => data.VenturerType == type);
        public static VenturerData GetData(DataIndex type) => CharacterDataList.TryGetElement(data => data.DataIndex       == type);
        public static IEquipment GetVenturerWeapon(VenturerType adventurer) => GetData(adventurer).GetWeapon();
        public static IEquipment GetVenturerArmor(VenturerType adventurer) => GetData(adventurer).GetAmor();

        /*
         * Inventory
         */ 
        public static int GetGrowMaterialCount(GrowMaterialType type) => GrowMaterialInventory.Count(type);
        public static void AddGrowMaterial(GrowMaterialType type, int count) => GrowMaterialInventory.Add(type, count);
        public static void ConsumeGrowMaterial(GrowMaterialType type, int count) => GrowMaterialInventory.Consume(type, count);


        public void Save()
        {
            characterDataList.ForEach(data => data.Save());
            growMaterialInventory.Save();
        }

        public void Load()
        {
            characterDataList.ForEach(data => data.Load());
            growMaterialInventory.Load();
        }
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            Finder.TryGetObjectList(out characterDataList);
        }
#endif
        
    }
}
