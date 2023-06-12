using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Databases
{
    public class PrefabData : ScriptableObject, IEditable
    {
        [FolderPath]
        [SerializeField] protected string prefabPath;
        [SerializeField] protected Table<DataIndex, GameObject> table;

        public bool GetObject(DataIndex index, out GameObject prefab)
        {
            prefab = table[index];
            
            return prefab.HasObject(); 
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
            if (!GetObject(index, out var result))
            {
                Debug.LogError($"Not Exist Prefab. Input:{index}");
                component = null;
                return false;
            }

            return result.TryGetComponent(out component);
        }

        public T Get<T>(DataIndex index) where T : MonoBehaviour
        {
            if (!GetObject(index, out var prefab))
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
            
            table.Clear();

            // Exclude Prefab Name start "_".
            // "_" is mean abstract prefab;
            prefabs.RemoveNull();
            prefabs.RemoveAll(prefab => prefab.name.StartsWith("_"));
            prefabs.ForEach(prefab =>
            {
                if (!prefab.TryGetComponent(out IDataIndexer indexer) || 
                    !prefab.TryGetComponent(out IEditable setter)) return;

                setter.EditorSetUp();
                table.Add(indexer.DataIndex, prefab);
                UnityEditor.EditorUtility.SetDirty(prefab);
            });
            
            UnityEditor.AssetDatabase.Refresh();
        }

        [Button(ButtonSizes.Large)]
        public void ResetTable() => table.Clear();
#endif
    }
}
