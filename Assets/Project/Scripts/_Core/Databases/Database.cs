using System.Collections.Generic;
using Databases;
using Databases.PrefabData;
using CombatClass = Databases.SheetData.ContentData.CombatClassData.CombatClass;
using Boss = Databases.SheetData.ContentData.BossData.Boss;
using Skill = Databases.SheetData.CombatData.SkillData.Skill;
using StatusEffect = Databases.SheetData.CombatData.StatusEffectData.StatusEffect;
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
        
    public static SkillMaster SkillMaster => Instance.skillMaster;
    public static StatusEffectMaster StatusEffectMaster => Instance.statusEffectMaster;
    public static CombatClassMaster CombatClassMaster => Instance.combatClassMaster;
    public static BossMaster BossMaster => Instance.bossMaster;
}