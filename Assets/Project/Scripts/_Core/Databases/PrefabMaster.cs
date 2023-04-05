using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Databases
{
    public class PrefabMaster : ScriptableObject, IEditable
    {
        [FolderPath]
        [SerializeField] protected string prefabPath;
        [SerializeField] protected List<GameObject> prefabList = new();

        private Dictionary<DataIndex, GameObject> prefabTable = new();
        
        [ShowInInspector]
        private Dictionary<DataIndex, GameObject> PrefabTable
        {
            get
            {
                if (prefabTable.IsNullOrEmpty() || prefabList.Count != prefabTable.Count)
                {
                    prefabTable = new Dictionary<DataIndex, GameObject>();
                    prefabList.ForEach(prefab =>
                    {
                        if (prefab.IsNullOrEmpty()) return;
                        if (!prefab.TryGetComponent(out IDataIndexer indexer)) return;
                        
                        prefabTable.TryAdd(indexer.ActionCode, prefab);
                    });
                }

                return prefabTable;
            }
        }
        
        
        public bool GetObject(DataIndex index, out GameObject prefab)
        {
            if (!PrefabTable.TryGetValue(index, out prefab))
            {
                Debug.LogError($"Not Exist Prefab. Input:{index}");
            }

            return prefab != null;
        }

        public bool Gets(List<DataIndex> index, out List<GameObject> prefabs)
        {
            var result = new List<GameObject>();
            
            index.ForEach(x =>
            {
                if (!GetObject(x, out var prefab)) return;
                
                result.Add(prefab);
            });

            prefabs = result;

            return !prefabs.IsNullOrEmpty();
        }
        
        public bool Get<T>(DataIndex index, out T component) where T : MonoBehaviour
        {
            if (!PrefabTable.TryGetValue(index, out var prefab))
            {
                Debug.LogError($"Not Exist Prefab. Input:{index}");
                component = null;
                return false;
            }

            return prefab.TryGetComponent(out component);
        }

        public T Get<T>(DataIndex index) where T : MonoBehaviour
        {
            if (!PrefabTable.TryGetValue(index, out var prefab))
            {
                Debug.LogError($"Not Exist Prefab. Input:{index}");
                return null;
            }

            if (!prefab.TryGetComponent(out T result))
            {
                Debug.LogError($"Not Exist {typeof(T).Name} in {index} prefab.");
                return null;
            }

            return result;
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (!Finder.TryGetObjectList(prefabPath, "", out List<GameObject> prefabs, true)) return;
            
            // Exclude Prefab Name start "_". "_" is mean abstract prefab;
            prefabs.RemoveNull();
            prefabs.RemoveAll(prefab => prefab.name.StartsWith("_"));
            
            prefabList.Clear();
            prefabList.AddRange(prefabs);
            
            PrefabTable.Values.ForEach(prefab =>
            {
                if (!prefab.TryGetComponent(out IEditable setter)) return;
                
                setter.EditorSetUp();
                UnityEditor.EditorUtility.SetDirty(prefab);
            });
            
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}
