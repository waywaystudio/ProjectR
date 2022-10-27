using UnityEngine;
using UnityEngine.Events;
using Wayway.Engine.Events.Core;

namespace Wayway.Engine.Save.Core
{
    public class SaveEventListener : GameEventListenerCore
    {
        [SerializeField] private UnityEvent saveEvent;
        [SerializeField] private UnityEvent loadEvent;

        public void SaveInvoke() => saveEvent?.Invoke();
        public void LoadInvoke() => loadEvent?.Invoke();
        
        protected new void OnEnable()
        {
            LoadInvoke();
            SaveManager.SaveEvent.Register(this);
        }

        protected new void OnDisable()
        {
            SaveInvoke();
            SaveManager.SaveEvent.Unregister(this);
        }
    }
}
