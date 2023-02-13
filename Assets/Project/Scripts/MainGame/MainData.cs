#if UNITY_EDITOR
using System.IO;
using System.Reflection;
#endif

using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace MainGame
{
    using Data;
    using ProtocolData = Data.GlobalData.ProtocolData.Protocol;
    using CombatClassData = Data.ContentData.CombatClassData.CombatClass;
    using SkillData = Data.CombatData.SkillData.Skill;
    using BossData = Data.ContentData.BossData.Boss;

    public class MainData : Core.Singleton.MonoSingleton<MainData>
    {
        [SerializeField] private List<DataObject> sheetDataList = new();
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
        
        public static CombatClassData GetCombatClass(DataIndex dataIndex) => SheetDataTable[DataIndex.CombatClass].Get<CombatClassData>(dataIndex);
        public static SkillData GetSkill(DataIndex dataIndex) => SheetDataTable[DataIndex.Skill].Get<SkillData>(dataIndex);
        public static BossData GetBoss(DataIndex dataIndex) => SheetDataTable[DataIndex.Boss].Get<BossData>(dataIndex);

#if UNITY_EDITOR
        
        [SerializeField] private string idCodePath;
        [SerializeField] private string dataScriptPath;
        [SerializeField] private string dataObjectPath;

        private void SetUp()
        {
            // UpdateDataList();
            CreateAndUpdateDataObjects();
            GenerateIDCode();
        }

        private void UpdateDataList()
        {
            sheetDataList.Clear();
            
            Finder.TryGetObjectList(out sheetDataList);
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
#endif
    }
}
