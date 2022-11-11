using GameEvent.Listener;
using UnityEngine;
using UnityEngine.Events;
using Wayway.Engine;

namespace Main.Save
{
    public class SaveEventListener : GameEventListener
    {
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
            if (MainGame.Instance is null) return;
            
            SaveInvoke();
            targetEvent.Unregister(this);
        }

#if UNITY_EDITOR
        private void OnInitialize()
        {
            targetEvent ??= ScriptableObjectUtility.GetScriptableObject<GameEvent.GameEvent>
            ("Assets/Project/Data/GameEvent/Save", "SaveEvent");
        }
#endif
    }
}
