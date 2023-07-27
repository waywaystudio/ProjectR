#if UNITY_EDITOR
using System.Reflection;
#endif

using UnityEngine;
using UnityEngine.Events;

namespace Serialization
{
    public class SaveListener : MonoBehaviour
    {
        [SerializeField] private SaveManager saveManager;
        [SerializeField] protected UnityEvent saveResponse;
        [SerializeField] protected UnityEvent loadResponse;

        public void Save() => saveResponse?.Invoke(); 
        public void Load() => loadResponse?.Invoke(); 

        private void OnEnable()
        {
            Load();
            saveManager.AddListener(this);
        }
        
        private void OnDisable()
        {
            Save();
            saveManager.RemoveListener(this);
        }

        private void Reset()
        {
#if UNITY_EDITOR
            AutoRegister();
#endif
        }
        
        
#if UNITY_EDITOR
        private void AutoRegister()
        {
            if (saveManager == null)
            {
                Finder.TryGetObject(out saveManager);
            }

            var behaviours = GetComponents<MonoBehaviour>();
            
            saveResponse.ClearAllUnityEventInEditor();
            loadResponse.ClearAllUnityEventInEditor();

            foreach (var behaviour in behaviours)
            {
                // Skip if the behaviour is the SaveListener itself
                if (behaviour == this)
                    continue;
                
                var saveMethod = behaviour.GetType().GetMethod("Save", BindingFlags.Public | BindingFlags.Instance);
                var loadMethod = behaviour.GetType().GetMethod("Load", BindingFlags.Public | BindingFlags.Instance);

                if (saveMethod != null)
                {
                    saveResponse.AddPersistantListenerInEditor(behaviour, "Save");
                }

                if (loadMethod != null)
                {
                    loadResponse.AddPersistantListenerInEditor(behaviour, "Load");
                }
            }
        }
#endif
    }
}
