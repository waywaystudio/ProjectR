using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace MainGame
{
    using Data;
    using Adventurer = Data.ContentData.AdventurerData.Adventurer;
    using Skill = Data.ContentData.SkillData.Skill;
    using CombatClass = Data.ContentData.CharacterClassData.CharacterClass;
    
    public class MainData : Core.Singleton.MonoSingleton<MainData>
    {
        [SerializeField] private List<DataObject> dataObjectList;

        private readonly Dictionary<string, int> nameTable = new();
        private Dictionary<DataCategory, DataObject> categoryTable = new();
        
        public static List<DataObject> DataObjectList => Instance.dataObjectList;
        public static Dictionary<string, int> NameTable
        {
            get
            {
                if (Instance.nameTable.IsNullOrEmpty()) DataObjectList.ForEach(x => x.RegisterNameTable(Instance.nameTable));

                return Instance.nameTable;
            }
        }
        public static Dictionary<DataCategory, DataObject> CategoryTable
        {
            get
            {
                if (Instance.categoryTable.IsNullOrEmpty()) Instance.categoryTable = DataObjectList.ToDictionary(x => x.Category);

                return Instance.categoryTable;
            }
        }
        

        public static T GetData<T>(DataCategory category, int id) where T : class 
            => TryGetData(category, id, out T result) ? result : null;
        public static T GetData<T>(DataCategory category, string nameKey) where T : class 
            => TryGetData(category, nameKey, out T result) ? result : null;

        public static Adventurer GetAdventurerData(string nameKey) => GetData<Adventurer>(DataCategory.Adventurer, nameKey);
        public static Adventurer GetAdventurerData(int id) => GetData<Adventurer>(DataCategory.Adventurer, id);
        public static Skill GetSkillData(string nameKey) => GetData<Skill>(DataCategory.Skill, nameKey);
        public static Skill GetSkillData(int id) => GetData<Skill>(DataCategory.Skill, id);
        public static CombatClass GetCombatClassData(string nameKey) => GetData<CombatClass>(DataCategory.CombatClass, nameKey);
        public static CombatClass GetCombatClassData(int id) => GetData<CombatClass>(DataCategory.CombatClass, id);
        // AddMoreDataSet...


        private static bool TryGetData<T>(DataCategory category, string nameKey, out T result) where T : class
        {
            var hasCategory = CategoryTable.TryGetValue(category, out var dataObject);
            var hasKey = NameTable.TryGetValue(nameKey, out var id);

            if (!hasCategory || !hasKey)
            {
                Debug.LogError($"No Key in Database. Key : {nameKey}");
                result = null;
                return false;
            }

            return dataObject.TryGetData(id, out result);
        }
        
        private static bool TryGetData<T>(DataCategory category, int id, out T result) where T : class
        {
            if (!CategoryTable.TryGetValue(category, out var dataObject))
            {
                Debug.LogError($"No Category in Database. Key : {category}");
                result = null;
                return false;
            }

            return dataObject.TryGetData(id, out result);
        }
        
        

#if UNITY_EDITOR
        private const string DataPath = "Assets/Project/Data/SpreadSheet/";

        public static bool EditorTryGetValue<T>(DataCategory category, string nameKey, out T result) where T : class
        {
            NameTable.TryGetValue(nameKey, out var id);
            
            return DataObjectList.Find(x => x.Category == category).TryGetData(id, out result);
        }
        
        private void GetAllData()
        {
            Finder.TryGetObjectList(DataPath, "Data", out dataObjectList);
        }

        private void OpenSpreadSheetPanel()
        {
            UnityEditor.EditorApplication.ExecuteMenuItem("Tools/UnityGoogleSheet");
        }
#endif
    }
}
