using GameEvent.Listener;
using UnityEngine;
using UnityEngine.Events;

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

        private void Reset()
        {
            if (targetEvent is null)
            {
                Finder.TryGetObject("Assets/Project/Data/GameEvent/Save", "SaveEvent", out targetEvent);
            }
        }
    }
}
