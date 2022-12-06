using System.Collections.Generic;
using MainGame.Data;
using MainGame.Data.ContentData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MainGame
{
    public class MainData : Core.Singleton.MonoSingleton<MainData>
    {
        [SerializeField] private List<DataObject> dataObjectList;
        
        [SerializeField] private AdventurerData adventurerData;
        [SerializeField] private SkillData skillData;
        [SerializeField] private RaidData raidData;
        
        private static readonly Dictionary<string, int> NameTable = new();

        // public bool TryGetAdventurerData(string nameKey, out AdventurerData.Adventurer result)
        //     => TryGetData(adventurerData, nameKey, out result);

        protected override void Awake()
        {
            base.Awake();

            dataObjectList.ForEach(x => x.RegisterNameTable(NameTable));
        }

        private bool TryGetData<T>(DataCategory category, string nameKey, out T result) where T : class
        {
            var dataObject = dataObjectList.Find(x => x.Category == category);

            if (dataObject is null)
            {
                Debug.LogError($"Not Exist Data : {category}");
                result = null;
                return false;
            }

            if (NameTable.TryGetValue(nameKey, out var id))
            {
                return dataObject.TryGetData(id, out result);
            }
            
            Debug.LogError($"No Key in Database. Key : {nameKey}");
            
            result = null;
            return false;
        }

#if UNITY_EDITOR
        [Button]
        private void GetAllDataObjects()
        {
            Finder.TryGetObjectList(out dataObjectList);
        }
#endif
    }
}
