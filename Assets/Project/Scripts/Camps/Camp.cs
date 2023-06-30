using System.Collections.Generic;
using Camps.Inventories;
using Character.Venturers;
using Serialization;
using Singleton;
using UnityEngine;
using Common;
// ReSharper disable CheckNamespace


public class Camp : MonoSingleton<Camp>, ISavable, IEditable
{
    [SerializeField] private List<VenturerType> challengers;
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
    public static bool GetVenturerPrefab(VenturerType type, out GameObject prefab) => Database.VenturerPrefabData.GetObject((DataIndex)type, out prefab);

    public static void Spawn(VenturerType venturer, Vector3 position, Transform hierarchy, out VenturerBehaviour vb)
    {
        vb = null;
        
        if (!GetVenturerPrefab(venturer, out var adventurerPrefab)) return;

        var adventurer = Instantiate(adventurerPrefab, position, Quaternion.identity, hierarchy);
        adventurer.SetActive(true);

        if (!adventurer.TryGetComponent(out vb)) return;
        
        vb.ForceInitialize();
    }

    /*
     * Inventory
     */ 
    public static int GetGrowMaterialCount(GrowMaterialType type) => GrowMaterialInventory.Count(type);
    public static void AddGrowMaterial(GrowMaterialType type, int count) => GrowMaterialInventory.Add(type, count);
    public static void AddGrowMaterial(GrowIngredient ingredient) => AddGrowMaterial(ingredient.Type, ingredient.Count);
    public static void AddGrowMaterials(IEnumerable<GrowIngredient> ingredients) 
        => ingredients.ForEach(ingredient => AddGrowMaterial(ingredient.Type, ingredient.Count));
    public static void ConsumeGrowMaterial(GrowMaterialType type, int count) => GrowMaterialInventory.Consume(type, count);
    
    /*
     * Stage Initializer
     */
    public static List<VenturerType> Challengers { get => Instance.challengers; set => Instance.challengers = value; }


    public void Save()
    {
        SaveManager.Save("challengers", challengers);
        // venturerTable.Iterate(venturer => venturer.Save()); 
        // growMaterialInventory.Save();
    }

    public void Load()
    {
        Challengers = SaveManager.Load("challengers", challengers);
        
        // venturerTable.Iterate(venturer => venturer.Load());
        // growMaterialInventory.Load();
    }
    
#if UNITY_EDITOR
    public void EditorSetUp()
    {
        challengers.Clear();
        challengers.AddRange(new List<VenturerType>
        {
            VenturerType.Knight, 
            VenturerType.Warrior, 
            VenturerType.Rogue, 
            VenturerType.Ranger, 
            VenturerType.Mage, 
            VenturerType.Priest
        });
        Finder.TryGetObjectList<VenturerData>(out var venturerDataList);
        
        venturerTable.CreateTable(venturerDataList, data => data.DataIndex);
    }

    public void EditorAddMaterial()
    {
        growMaterialInventory.AddAll100Material();
    }
#endif
    
}

