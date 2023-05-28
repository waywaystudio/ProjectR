using System.Collections.Generic;
using Databases;
using Databases.Prefabs;
using Databases.ResourceData;
using Venturer = Databases.SheetData.CharacterData.VenturerData.Venturer;
using Villain = Databases.SheetData.CharacterData.VillainData.Villain;
using Relic = Databases.SheetData.ContentData.RelicData.Relic;
using Skill = Databases.SheetData.CombatData.SkillData.Skill;
using StatusEffect = Databases.SheetData.CombatData.StatusEffectData.StatusEffect;
using Weapon = Databases.SheetData.EquipmentData.WeaponData.Weapon;
using Head = Databases.SheetData.EquipmentData.HeadData.Head;
using Top = Databases.SheetData.EquipmentData.TopData.Top;
using Glove = Databases.SheetData.EquipmentData.GloveData.Glove;
using Bottom = Databases.SheetData.EquipmentData.BottomData.Bottom;
using Boot = Databases.SheetData.EquipmentData.BootData.Boot;
using Trinket = Databases.SheetData.EquipmentData.TrinketData.Trinket;
using Material = Databases.SheetData.ItemData.MaterialData.Material;
using Singleton;
using UnityEngine;

// ReSharper disable CheckNamespace

public partial class Database : MonoSingleton<Database>, IEditable
{
    [SerializeField] private List<DataObject> sheetDataList = new();
    
    [SerializeField] private SkillPrefabData skillPrefabData;
    [SerializeField] private StatusEffectPrefabData statusEffectPrefabData;
    [SerializeField] private CombatClassPrefabData combatClassPrefabData;
    [SerializeField] private BossPrefabData bossPrefabData;

    [SerializeField] private SpriteData equipmentSpriteData;
    [SerializeField] private SpriteData spellSpriteData;
    [SerializeField] private SpriteData materialSpriteData;
        
    private readonly Dictionary<DataIndex, DataObject> sheetDataTable = new();

    public static List<DataObject> SheetDataList => Instance.sheetDataList;
    public static Dictionary<DataIndex, DataObject> SheetDataTable
    {
        get
        {
            if (Instance.sheetDataTable.IsNullOrEmpty()) 
                Instance.sheetDataList.ForEach(x => Instance.sheetDataTable.TryAdd(x.Index, x));
            return Instance.sheetDataTable;
        }
    }

    public static Venturer CombatClassSheetData(DataIndex      dataIndex) => SheetDataTable[DataIndex.Venturer].Get<Venturer>(dataIndex);
    public static Villain BossSheetData(DataIndex              dataIndex) => SheetDataTable[DataIndex.Villain].Get<Villain>(dataIndex);
    public static Skill SkillSheetData(DataIndex               dataIndex) => SheetDataTable[DataIndex.Skill].Get<Skill>(dataIndex);
    public static StatusEffect StatusEffectSheetData(DataIndex dataIndex) => SheetDataTable[DataIndex.StatusEffect].Get<StatusEffect>(dataIndex);
    public static Weapon WeaponData(DataIndex                  dataIndex) => SheetDataTable[DataIndex.Weapon].Get<Weapon>(dataIndex);
    public static Head HeadData(DataIndex                      dataIndex) => SheetDataTable[DataIndex.Head].Get<Head>(dataIndex);
    public static Top TopData(DataIndex                        dataIndex) => SheetDataTable[DataIndex.Top].Get<Top>(dataIndex);
    public static Glove GloveData(DataIndex                    dataIndex) => SheetDataTable[DataIndex.Glove].Get<Glove>(dataIndex);
    public static Bottom BottomData(DataIndex                  dataIndex) => SheetDataTable[DataIndex.Bottom].Get<Bottom>(dataIndex);
    public static Boot BootData(DataIndex                      dataIndex) => SheetDataTable[DataIndex.Boot].Get<Boot>(dataIndex);
    public static Trinket TrinketData(DataIndex                dataIndex) => SheetDataTable[DataIndex.Trinket].Get<Trinket>(dataIndex);
    public static Material MaterialData(DataIndex              dataIndex) => SheetDataTable[DataIndex.Material].Get<Material>(dataIndex);
    public static Relic RelicData(DataIndex                    dataIndex) => SheetDataTable[DataIndex.Relic].Get<Relic>(dataIndex);
        
    public static SkillPrefabData SkillPrefabData => Instance.skillPrefabData;
    public static StatusEffectPrefabData StatusEffectPrefabData => Instance.statusEffectPrefabData;
    public static CombatClassPrefabData CombatClassPrefabData => Instance.combatClassPrefabData;
    public static BossPrefabData BossPrefabData => Instance.bossPrefabData;

    public static SpriteData EquipmentSpriteData => Instance.equipmentSpriteData;
    public static SpriteData SpellSpriteData => Instance.spellSpriteData;
    public static SpriteData MaterialSpriteData => Instance.materialSpriteData;
}