using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;


namespace MainGame.Data.PrefabData
{
    public class SkillMaster : ScriptableObject
    {
        [FolderPath]
        [SerializeField] private string prefabPath;
        [SerializeField] private List<GameObject> skillPrefabList;
        
        private Dictionary<DataIndex, GameObject> skillTable;

        [ShowInInspector]
        private Dictionary<DataIndex, GameObject> SkillTable
        {
            get
            {
                if (skillTable.IsNullOrEmpty())
                {
                    skillTable = new Dictionary<DataIndex, GameObject>();
                    skillPrefabList.ForEach(skillPrefab =>
                    {
                        if (!skillPrefab.TryGetComponent(out IDataIndexer indexer)) return;
                        skillTable.TryAdd(indexer.ActionCode, skillPrefab);
                    });
                }

                return skillTable;
            }
        }

        public bool Get(DataIndex index, out GameObject skillComponent)
        {
            if (!SkillTable.TryGetValue(index, out var result))
            {
                Debug.LogError($"Not Exist SkillComponent. Input:{index}");
                skillComponent = null;
            }
            else
                skillComponent = result;

            return skillComponent != null;
        }

        // TODO. Main Assembly에서 접근 불가. 받고 싶다면, ISkill을 Core에서 구현한 후, Interface 형태로 받기.
        // public bool TryGetSkillComponent(DataIndex index, out SkillComponent skill) { }


#if UNITY_EDITOR
        public void SetUp()
        {
            if (!Finder.TryGetObjectList(prefabPath, "", out List<GameObject> skillPrefabs, true)) return;
            
            skillPrefabList.Clear();
            skillPrefabList.AddRange(skillPrefabs);
            
            SkillTable.Values.ForEach(skillPrefab =>
            {
                if (!skillPrefab.TryGetComponent(out IEditable setter)) return;
                
                setter.EditorSetUp();
                UnityEditor.EditorUtility.SetDirty(skillPrefab);
                // UnityEditor.PrefabUtility.ApplyPrefabInstance(skillPrefab, UnityEditor.InteractionMode.AutomatedAction);
            });
            
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}
