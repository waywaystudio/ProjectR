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
        [SerializeField] private Table<DataIndex, VenturerData> venturerTable = new();

        private readonly GrowMaterialInventory growMaterialInventory = new();
        private static Table<DataIndex, VenturerData> VenturerTable => Instance.venturerTable;
        private static GrowMaterialInventory GrowMaterialInventory => Instance.growMaterialInventory;

        /*
         * Venturer
         */
        public static VenturerData GetData(VenturerType type) => VenturerTable[(DataIndex)type];
        public static VenturerData GetData(DataIndex type) => VenturerTable[type];
        public static IEquipment GetVenturerWeapon(VenturerType adventurer) => GetData(adventurer).GetWeapon();
        public static IEquipment GetVenturerArmor(VenturerType adventurer) => GetData(adventurer).GetAmor();

        /*
         * Inventory
         */ 
        public static int GetGrowMaterialCount(GrowMaterialType type) => GrowMaterialInventory.Count(type);
        public static void AddGrowMaterial(GrowMaterialType type, int count) => GrowMaterialInventory.Add(type, count);
        public static void AddGrowMaterial(GrowIngredient ingredient) => AddGrowMaterial(ingredient.Type, ingredient.Count);
        public static void AddGrowMaterials(IEnumerable<GrowIngredient> ingredients) 
            => ingredients.ForEach(ingredient => AddGrowMaterial(ingredient.Type, ingredient.Count));
        public static void ConsumeGrowMaterial(GrowMaterialType type, int count) => GrowMaterialInventory.Consume(type, count);


        public void Save()
        {
            venturerTable.Iterate(venturer => venturer.Save()); 
            growMaterialInventory.Save();
        }

        public void Load()
        {
            venturerTable.Iterate(venturer => venturer.Load());
            growMaterialInventory.Load();
        }
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            Finder.TryGetObjectList<VenturerData>(out var venturerDataList);
            
            venturerTable.CreateTable(venturerDataList, data => data.DataIndex);
        }

        public void EditorAddMaterial()
        {
            growMaterialInventory.AddAll100Material();
        }
#endif
        
    }
}
