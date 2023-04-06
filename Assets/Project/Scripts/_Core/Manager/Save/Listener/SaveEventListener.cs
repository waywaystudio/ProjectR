using GameEvents.Listener;
using UnityEngine;
using UnityEngine.Events;

namespace Manager.Save.Listener
{
    public class SaveEventListener : GameEventListener, IEditable
    {
        // # GameEvent targetEvent;
        // # int priority = 5;
        [SerializeField] private UnityEvent loadEvent;

        public void SaveInvoke() => Invoke();
        public void LoadInvoke() => loadEvent?.Invoke();
        
        protected new void OnEnable()
        {
            LoadInvoke();
            targetEvent.Register(this);
        }

        protected new void OnDisable()
        {
            SaveInvoke();
            targetEvent.Unregister(this);
        }

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (targetEvent is null)
                Finder.TryGetObject("Assets/Project/Data/GameEvent/Save", "OnSaved", out targetEvent);

            if(!TryGetComponent(out ISavable savable))
            {
                return;
            } 
            
            response.ClearUnityEventInEditor();
            response.AddPersistantListenerInEditor(savable, "Save");
            
            loadEvent.ClearUnityEventInEditor();
            loadEvent.AddPersistantListenerInEditor(savable, "Load");
        }
#endif
    }
}
