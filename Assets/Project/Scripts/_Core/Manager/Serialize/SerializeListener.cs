using Manager.Save;
using UnityEngine;
using UnityEngine.Events;

namespace Manager.Serialize
{
    public class SerializeListener : MonoBehaviour, IEditable
    {
        [SerializeField] private SerializeManager serializer;
        [SerializeField] private UnityEvent saveEvent;
        [SerializeField] private UnityEvent loadEvent;
        
        
        public void Save()
        {
            saveEvent?.Invoke();
        }

        public void Load()
        {
            loadEvent?.Invoke();
        }

        private void OnEnable()
        {
            Load();
            serializer.AddListener(this);
        }
        
        private void OnDisable()
        {
            Save();
            serializer.RemoveListener(this);
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            serializer ??= Resources.Load<SerializeManager>("SerializeManager");

            if(!TryGetComponent(out ISavable savable))
            {
                return;
            } 
            
            saveEvent.ClearUnityEventInEditor();
            saveEvent.AddPersistantListenerInEditor(savable, "Save");
            
            loadEvent.ClearUnityEventInEditor();
            loadEvent.AddPersistantListenerInEditor(savable, "Load");
        }
#endif
    }
}
