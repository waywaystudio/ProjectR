using UnityEngine;
using UnityEngine.Events;

namespace Serialization
{
    public class SaveListener : MonoBehaviour, IEditable
    {
        [SerializeField] private UnityEvent saveEvent;
        [SerializeField] private UnityEvent loadEvent;

        public void Save() => saveEvent?.Invoke();
        public void Load() => loadEvent?.Invoke();

        private void OnEnable()
        {
            Load();
            SaveManager.Instance.AddListener(this);
        }
        
        private void OnDisable()
        {
            Save();
            SaveManager.Instance.RemoveListener(this);
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
