using System.Collections.Generic;
using Databases;
using Databases.PrefabData;
using CombatClass = Databases.SheetData.ContentData.CombatClassData.CombatClass;
using Boss = Databases.SheetData.ContentData.BossData.Boss;
using Skill = Databases.SheetData.CombatData.SkillData.Skill;
using StatusEffect = Databases.SheetData.CombatData.StatusEffectData.StatusEffect;
using Weapon = Databases.SheetData.EquipmentData.WeaponData.Weapon;
using Head = Databases.SheetData.EquipmentData.HeadData.Head;
using Top = Databases.SheetData.EquipmentData.TopData.Top;
using Bottom = Databases.SheetData.EquipmentData.BottomData.Bottom;
using Trinket = Databases.SheetData.EquipmentData.TrinketData.Trinket;
using Singleton;
using UnityEngine;

// ReSharper disable CheckNamespace

public partial class Database : MonoSingleton<Database>
{
    [SerializeField] private List<DataObject> sheetDataList = new();
    [SerializeField] private SkillMaster skillMaster;
    [SerializeField] private StatusEffectMaster statusEffectMaster;
    [SerializeField] private CombatClassMaster combatClassMaster;
    [SerializeField] private BossMaster bossMaster;
    [SerializeField] private EquipmentMaster equipmentMaster;
        
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

    public static CombatClass CombatClassSheetData(DataIndex dataIndex) => SheetDataTable[DataIndex.CombatClass].Get<CombatClass>(dataIndex);
    public static Boss BossSheetData(DataIndex dataIndex) => SheetDataTable[DataIndex.Boss].Get<Boss>(dataIndex);
    public static Skill SkillSheetData(DataIndex dataIndex) => SheetDataTable[DataIndex.Skill].Get<Skill>(dataIndex);
    public static StatusEffect StatusEffectSheetData(DataIndex dataIndex) => SheetDataTable[DataIndex.StatusEffect].Get<StatusEffect>(dataIndex);
    public static Weapon WeaponData(DataIndex dataIndex) => SheetDataTable[DataIndex.Weapon].Get<Weapon>(dataIndex);
    public static Head HeadData(DataIndex dataIndex) => SheetDataTable[DataIndex.Head].Get<Head>(dataIndex);
    public static Top TopData(DataIndex dataIndex) => SheetDataTable[DataIndex.Top].Get<Top>(dataIndex);
    public static Bottom BottomData(DataIndex dataIndex) => SheetDataTable[DataIndex.Bottom].Get<Bottom>(dataIndex);
    public static Trinket TrinketData(DataIndex dataIndex) => SheetDataTable[DataIndex.Trinket].Get<Trinket>(dataIndex);
        
    public static SkillMaster SkillMaster => Instance.skillMaster;
    public static StatusEffectMaster StatusEffectMaster => Instance.statusEffectMaster;
    public static CombatClassMaster CombatClassMaster => Instance.combatClassMaster;
    public static BossMaster BossMaster => Instance.bossMaster;
    public static EquipmentMaster EquipmentMaster => Instance.equipmentMaster;
}