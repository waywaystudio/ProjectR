using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MainGame.Data.PrefabData
{
    public class CombatClassMaster : ScriptableObject
    {
        [FolderPath]
        [SerializeField] private string prefabPath;
        [SerializeField] private List<GameObject> combatClassPrefabList;
        
        private Dictionary<DataIndex, GameObject> combatClassTable;

        [ShowInInspector]
        private Dictionary<DataIndex, GameObject> CombatClassTable
        {
            get
            {
                if (combatClassTable.IsNullOrEmpty())
                {
                    combatClassTable = new Dictionary<DataIndex, GameObject>();
                    combatClassPrefabList.ForEach(skillPrefab =>
                    {
                        if (!skillPrefab.TryGetComponent(out IDataIndexer indexer)) return;
                        combatClassTable.TryAdd(indexer.ActionCode, skillPrefab);
                    });
                }

                return combatClassTable;
            }
        }
        
        public bool Get(DataIndex index, out GameObject combatClassComponent)
        {
            if (!CombatClassTable.TryGetValue(index, out var result))
            {
                Debug.LogError($"Not Exist CombatClassComponent. Input:{index}");
                combatClassComponent = null;
            }
            else
                combatClassComponent = result;

            return combatClassComponent != null;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (!Finder.TryGetObjectList(prefabPath, "", out List<GameObject> combatClassPrefabs, true)) return;
            
            combatClassPrefabList.Clear();
            combatClassPrefabList.AddRange(combatClassPrefabs);
            
            CombatClassTable.Values.ForEach(skillPrefab =>
            {
                if (!skillPrefab.TryGetComponent(out IEditable setter)) return;
                
                setter.EditorSetUp();
                UnityEditor.EditorUtility.SetDirty(skillPrefab);
            });
            
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}
