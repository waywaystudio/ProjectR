using GameEvents;
using GameEvents.Listener;
using UnityEngine;
using UnityEngine.Events;

namespace Manager.Save.Listener
{
    public class SaveEventListener : GameEventListener, IEditable
    {
        // # GameEvent targetEvent;
        // # int priority = 5;
        // # UnityEvent response;
        [SerializeField] private GameEvent loadEvent; 
        [SerializeField] private UnityEvent loadResponse;

        public void SaveInvoke() => Invoke();
        public void LoadInvoke() => loadResponse?.Invoke();
        
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
            
            loadResponse.ClearUnityEventInEditor();
            loadResponse.AddPersistantListenerInEditor(savable, "Load");
        }
#endif
    }
}
