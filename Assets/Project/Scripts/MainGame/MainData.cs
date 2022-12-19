using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEditor;
using UnityEngine;

namespace MainGame
{
    using Data;
    using Adventurer = Data.ContentData.AdventurerData.Adventurer;
    using Skill = Data.ContentData.SkillData.Skill;
    using CombatClass = Data.ContentData.CombatClassData.CombatClass;
    using Equipment = Data.ContentData.EquipmentData.Equipment;
    using StatusEffect = Data.ContentData.StatusEffectData.StatusEffect;

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
        public static Equipment GetEquipmentData(string nameKey) => GetData<Equipment>(DataCategory.Equipment, nameKey);
        public static Equipment GetEquipmentData(int id) => GetData<Equipment>(DataCategory.Equipment, id);
        public static StatusEffect GetStatusEffectData(string nameKey) => GetData<StatusEffect>(DataCategory.StatusEffect, nameKey);
        public static StatusEffect GetStatusEffectData(int id) => GetData<StatusEffect>(DataCategory.StatusEffect, id);

        // AddMoreDataSet...


        private static bool TryGetData<T>(DataCategory category, string nameKey, out T result) where T : class
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var dataObj = DataObjectList.Find(x => x.Category == category);
                result = dataObj.EditorGetData<T>(nameKey);

                return result != null;
            }
#endif
            
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
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var dataObj = DataObjectList.Find(x => x.Category == category);
                result = dataObj.EditorGetData<T>(id);
            
                return result != null;
            }
#endif
            
            if (!CategoryTable.TryGetValue(category, out var dataObject))
            {
                Debug.LogError($"No Category in Database. Key : {category}");
                result = null;
                return false;
            }

            return dataObject.TryGetData(id, out result);
        }



#if UNITY_EDITOR
        #region MainDataDrawer.cs
        
        private const string DataPath = "Assets/Project/Data/SpreadSheet/";
        private void GetAllData() => Finder.TryGetObjectList(DataPath, "Data", out dataObjectList);
        private void OpenSpreadSheetPanel() => EditorApplication.ExecuteMenuItem("Tools/UnityGoogleSheet");

        #endregion
#endif
    }
}
