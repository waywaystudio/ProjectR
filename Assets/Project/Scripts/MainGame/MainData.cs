#if UNITY_EDITOR
using System.IO;
using System.Reflection;
#endif

using System.Collections.Generic;
using Core;
using Core.Singleton;
using UnityEngine;

namespace MainGame
{
    using Data;
    using Data.PrefabData;
    using ProtocolData = Data.SheetData.GlobalData.ProtocolData.Protocol;
    using CombatClassData = Data.SheetData.ContentData.CombatClassData.CombatClass;
    using SkillData = Data.SheetData.CombatData.SkillData.Skill;
    using StatusEffectData = Data.SheetData.CombatData.StatusEffectData.StatusEffect;
    using BossData = Data.SheetData.ContentData.BossData.Boss;

    public class MainData : MonoSingleton<MainData>
    {
        [SerializeField] private List<DataObject> sheetDataList = new();
        [SerializeField] private SkillMaster skillMaster;
        [SerializeField] private StatusEffectMaster statusEffectMaster;
        
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

        public static CombatClassData CombatClassSheetData(DataIndex dataIndex) => SheetDataTable[DataIndex.CombatClass].Get<CombatClassData>(dataIndex);
        public static SkillData SkillSheetData(DataIndex dataIndex) => SheetDataTable[DataIndex.Skill].Get<SkillData>(dataIndex);
        public static StatusEffectData StatusEffectSheetData(DataIndex dataIndex) => SheetDataTable[DataIndex.StatusEffect].Get<StatusEffectData>(dataIndex);
        public static BossData BossSheetData(DataIndex dataIndex) => SheetDataTable[DataIndex.Boss].Get<BossData>(dataIndex);
        
        public static SkillMaster SkillMaster => Instance.skillMaster;
        public static StatusEffectMaster StatusEffectMaster => Instance.statusEffectMaster;
        
        

#if UNITY_EDITOR
        
        [SerializeField] private string iconPath;
        [SerializeField] private string idCodePath;
        [SerializeField] private string dataScriptPath;
        [SerializeField] private string dataObjectPath;
        // [SerializeField] private string dataMasterPath;
        
        /// <summary>
        /// Editor Function
        /// </summary>
        public static bool TryGetIcon(string iconName, out Sprite icon)
        {
            icon = !Finder.TryGetObject($"{Instance.iconPath}", $"{iconName}", out Sprite result, true)
                ? null
                : result;

            return icon is not null;
        }

        private void EditorSetUp()
        {
            CreateAndUpdateDataObjects();
            GenerateIDCode();
        }


        // SheetData Generator
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
            
            Finder.TryGetObjectList(out sheetDataList);

            sheetDataList.ForEach(x => x.Index = x.name.Replace("Data", "").ToEnum<DataIndex>());
            sheetDataList.Sort((dataA, dataB) => dataA.Index.CompareTo(dataB.Index));
            
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
        
        [UnityEditor.MenuItem("Quick Menu/Data")]
        private static void QuickMenu()
        {
            var mainGameObject = GameObject.Find("MainGame");
            if (mainGameObject != null)
            {
                mainGameObject.TryGetComponent(out MainData mainData);
                
                UnityEditor.EditorUtility.OpenPropertyEditor(mainData);
            }
            else
            {
                Debug.LogWarning("MainData GameObject not found in scene hierarchy.");
            }
            
        }
#endif
    }
}
