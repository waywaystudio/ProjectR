using Manager.Save;
using UnityEngine;
using UnityEngine.Events;

namespace Manager.Serialize
{
    public class SerializeListener : MonoBehaviour, IEditable
    {
        [SerializeField] private UnityEvent saveEvent;
        [SerializeField] private UnityEvent loadEvent;

        public void Save() => saveEvent?.Invoke();
        public void Load() => loadEvent?.Invoke();

        private void OnEnable()
        {
            Load();
            SerializeManager.Instance.AddListener(this);
        }
        
        private void OnDisable()
        {
            Save();
            SerializeManager.Instance.RemoveListener(this);
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if(!TryGetComponent(out ISavable savable))
            {
                return;
            }
            
            Debug.Log($"Successfully Find ISavable Interface on Same GameObject.");
            
            saveEvent.ClearUnityEventInEditor();
            saveEvent.AddPersistantListenerInEditor(savable, "Save");
            
            loadEvent.ClearUnityEventInEditor();
            loadEvent.AddPersistantListenerInEditor(savable, "Load");
        }
#endif
    }
}
