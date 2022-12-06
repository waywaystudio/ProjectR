using System.Collections.Generic;
using System.Linq;
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

        private static readonly Dictionary<string, int> NameTable = new();
        private static Dictionary<DataCategory, DataObject> categoryTable = new();
        
        
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

        protected override void Awake()
        {
            base.Awake();

            dataObjectList.ForEach(x => x.RegisterNameTable(NameTable));
            categoryTable = dataObjectList.ToDictionary(x => x.Category);
        }

        private static bool TryGetData<T>(DataCategory category, string nameKey, out T result) where T : class
        {
            var hasCategory = categoryTable.TryGetValue(category, out var dataObject);
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
            if (!categoryTable.TryGetValue(category, out var dataObject))
            {
                Debug.LogError($"No Category in Database. Key : {category}");
                result = null;
                return false;
            }

            return dataObject.TryGetData(id, out result);
        }

#if UNITY_EDITOR
        private const string DataPath = "Assets/Project/Data/SpreadSheet/";
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
