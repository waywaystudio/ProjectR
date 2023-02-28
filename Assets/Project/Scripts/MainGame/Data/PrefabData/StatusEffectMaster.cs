using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MainGame.Data.PrefabData
{
    public class StatusEffectMaster : ScriptableObject
    {
        [FolderPath]
        [SerializeField] private string prefabPath;
        [SerializeField] private List<GameObject> statusEffectPrefabList;
        
        private Dictionary<DataIndex, GameObject> statusEffectTable = new();

        [ShowInInspector]
        private Dictionary<DataIndex, GameObject> StatusEffectTable
        {
            get
            {
                if (statusEffectTable.IsNullOrEmpty())
                {
                    statusEffectTable = new Dictionary<DataIndex, GameObject>();
                    statusEffectPrefabList.ForEach(statusEffectPrefab =>
                    {
                        if (!statusEffectPrefab.TryGetComponent(out IDataIndexer indexer)) return;
                        statusEffectTable.TryAdd(indexer.ActionCode, statusEffectPrefab);
                    });
                }

                return statusEffectTable;
            }
        }
        
        public bool Get(DataIndex index, out GameObject statusEffectComponent)
        {
            if (!StatusEffectTable.TryGetValue(index, out var result))
            {
                Debug.LogError($"Not Exist StatusEffectComponent. Input:{index}");
                statusEffectComponent = null;
            }
            else
                statusEffectComponent = result;

            return statusEffectComponent != null;
        }
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (!Finder.TryGetObjectList(prefabPath, "", out List<GameObject> statusEffectPrefabs, true)) return;
            
            statusEffectPrefabList.Clear();
            statusEffectPrefabList.AddRange(statusEffectPrefabs);
            
            StatusEffectTable.Values.ForEach(statusEffectPrefab =>
            {
                if (!statusEffectPrefab.TryGetComponent(out IEditable setter)) return;
                
                setter.EditorSetUp();
                UnityEditor.EditorUtility.SetDirty(statusEffectPrefab);
            });
            
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}
