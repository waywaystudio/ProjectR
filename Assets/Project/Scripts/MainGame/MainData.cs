#if UNITY_EDITOR
using System.IO;
using System.Reflection;
#endif

using System.Collections.Generic;
using Core;
using UnityEngine;

namespace MainGame
{
    using Data;
    using AdventurerData = Data.ContentData.AdventurerData.Adventurer;
    using CombatClassData = Data.ContentData.CombatClassData.CombatClass;
    using SkillData = Data.ContentData.SkillData.Skill;
    using StatusEffectData = Data.ContentData.StatusEffectData.StatusEffect;
    using ProjectileData = Data.ContentData.ProjectileData.Projectile;
    using RaidData = Data.ContentData.RaidData.Raid;
    using BossData = Data.ContentData.BossData.Boss;
    using EquipmentData = Data.ContentData.EquipmentData.Equipment;

    public class MainData : Core.Singleton.MonoSingleton<MainData>
    {
        [SerializeField] private List<DataObject> dataList = new();
        private readonly Dictionary<int, DataObject> dataTable = new();

        public static List<DataObject> DataList => Instance.dataList;
        public static Dictionary<int, DataObject> DataTable
        {
            get
            {
                if (Instance.dataTable.IsNullOrEmpty()) 
                    Instance.dataList.ForEach(x => Instance.dataTable.TryAdd(x.Index, x));
                return Instance.dataTable;
            }
        }

        public static AdventurerData GetAdventurer(DataIndex dataIndex) => DataTable[11].Get<AdventurerData>(dataIndex);
        public static CombatClassData GetCombatClass(DataIndex dataIndex) => DataTable[12].Get<CombatClassData>(dataIndex);
        public static SkillData GetSkill(DataIndex dataIndex) => DataTable[13].Get<SkillData>(dataIndex);
        public static StatusEffectData GetStatusEffect(DataIndex dataIndex) => DataTable[14].Get<StatusEffectData>(dataIndex);
        public static ProjectileData GetProjectile(DataIndex dataIndex) => DataTable[15].Get<ProjectileData>(dataIndex);
        public static RaidData GetRaid(DataIndex dataIndex) => DataTable[16].Get<RaidData>(dataIndex);
        public static BossData GetBoss(DataIndex dataIndex) => DataTable[17].Get<BossData>(dataIndex);
        public static EquipmentData GetEquipment(DataIndex dataIndex) => DataTable[21].Get<EquipmentData>(dataIndex);
        

#if UNITY_EDITOR
        
        [SerializeField] private string idCodePath;
        [SerializeField] private string dataScriptPath;
        [SerializeField] private string dataObjectPath;

        private void SetUp()
        {
            CreateAndUpdateDataObjects();
            GenerateIDCode();
        }

        private void CreateAndUpdateDataObjects()
        {
            Finder.TryGetObjectList(dataScriptPath, $"t:MonoScript, Data", out List<UnityEditor.MonoScript> monoList);
            
            monoList.ForEach(x =>
            {
                if (!x.name.EndsWith("Data")) return;
                if (!Finder.TryGetObject(dataObjectPath, x.name, out ScriptableObject dataObject))
                {
                    dataObject = Finder.CreateScriptableObject(dataObjectPath, x.name, x.name);
                }

                var dataObjectType = dataObject.GetType();
                var info = dataObjectType.GetMethod("LoadFromJson", BindingFlags.NonPublic | BindingFlags.Instance);

                if (info != null)
                {
                    info.Invoke(dataObject, null);
                }

                UnityEditor.EditorUtility.SetDirty(dataObject);
            });
            
            Finder.TryGetObjectList(out dataList);
            dataList.Sort((dataA, dataB) => dataA.Index.CompareTo(dataB.Index));
            
            UnityEditor.AssetDatabase.Refresh();
        }
        private void GenerateIDCode()
        {
            if (!Directory.Exists(idCodePath))
                Directory.CreateDirectory(idCodePath);
            
            File.WriteAllText($"{idCodePath}/DataIndex.cs", DataIndexGenerator.Generate());
        }
        
        private void OpenSpreadSheetPanel() 
            => UnityEditor.EditorApplication.ExecuteMenuItem("Tools/UnityGoogleSheet");
#endif
    }
}
