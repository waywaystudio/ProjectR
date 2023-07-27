using System.Collections.Generic;
using Camps;
using Camps.Inventories;
using Camps.Storages;
using Character.Venturers;
using Singleton;
using UnityEngine;
using Common;
using Common.Runes;
using Common.Runes.Rewards;
// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault
// ReSharper disable CheckNamespace

public class Camp : MonoSingleton<Camp>, IEditable
{
    [SerializeField] private List<VenturerType> challengers;
    [SerializeField] private Table<DataIndex, VenturerData> venturerTable = new();
    [SerializeField] private PartyLevel partyLevel;
    [SerializeField] private RuneStorage runeStorage;
    [SerializeField] private GoldStorage goldStorage;

    private readonly GrowMaterialStorage growMaterialStorage = new();
    private static Table<DataIndex, VenturerData> VenturerTable => Instance.venturerTable;
    private static GrowMaterialStorage GrowMaterialStorage => Instance.growMaterialStorage;
    private static RuneStorage RuneStorage => Instance.runeStorage;
    private static GoldStorage GoldStorage => Instance.goldStorage;
    
    public static PartyLevel Level => Instance.partyLevel;

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
        
        vb.Initialize();
    }
    
    /*
     * Party Level
     */
    
    

    /*
     * Inventory
     */
    public static int GetGrowMaterialCount(GrowMaterialType type) => GrowMaterialStorage.Count(type);
    public static void AddGrowMaterial(GrowMaterialType type, int count) => GrowMaterialStorage.Add(type, count);
    public static void AddGrowMaterial(GrowIngredient ingredient) => AddGrowMaterial(ingredient.Type, ingredient.Count);
    public static void AddGrowMaterials(IEnumerable<GrowIngredient> ingredients) 
        => ingredients.ForEach(ingredient => AddGrowMaterial(ingredient.Type, ingredient.Count));
    public static void ConsumeGrowMaterial(GrowMaterialType type, int count) => GrowMaterialStorage.Consume(type, count);

    /*
     * Runes
     */
    public static void AddRunes(IEnumerable<EthosRune> runes) => RuneStorage.AddRunes(runes);
    public static void RevealEthosRune(EthosRune rune)
    {
        if (!rune.IsCompleteTask) return;

        var rewardRune = rune.RewardRune;

        switch (rewardRune.RuneType)
        {
            case RewardRuneType.Gold:
            {
                if (rewardRune is not GoldRune goldRune) return;

                GoldStorage.Gold += goldRune.RewardGold;
                break;
            }
            case RewardRuneType.Experience:
            {
                if (rewardRune is not ExperienceRune experienceRune) return;

                Level.Experience += experienceRune.Experience;
                break;
            }
            case RewardRuneType.Skill:
            {
                // Inventory.GetSkillRune(rune);
                break;
            }
        }
        
        RuneStorage.RemoveRune(rune);
    }
    
    /*
     * Stage Initializer
     */
    public static List<VenturerType> Challengers { get => Instance.challengers; set => Instance.challengers = value; }


    public void Save()
    {
        venturerTable.Iterate(venturer => venturer.Save()); 
        GrowMaterialStorage.Save();
        partyLevel.Save();
        runeStorage.Save();
        goldStorage.Save();
    }

    public void Load()
    {
        venturerTable.Iterate(venturer => venturer.Load());
        GrowMaterialStorage.Load();
        partyLevel.Load();
        runeStorage.Load();
        goldStorage.Load();
    }


#if UNITY_EDITOR
    public void EditorSetUp()
    {
        challengers.Clear();
        challengers.AddRange(new List<VenturerType>
        {
            VenturerType.Knight, 
            VenturerType.Warrior, 
            VenturerType.Ranger, 
            VenturerType.Priest
            // VenturerType.Rogue, 
            // VenturerType.Mage, 
        });
        Finder.TryGetObjectList<VenturerData>(out var venturerDataList);
        
        venturerTable.CreateTable(venturerDataList, data => data.DataIndex);
    }

    public void EditorAddMaterial()
    {
        GrowMaterialStorage.AddAll100Material();
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetSingleton()
    {
        if (!Instance.IsNullOrDestroyed())
            Instance.SetInstanceNull();
    }
#endif
    
}

