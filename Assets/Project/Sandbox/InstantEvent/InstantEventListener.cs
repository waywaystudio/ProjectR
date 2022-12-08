using UnityEngine;
using UnityEngine.Events;

namespace InstantEvent
{
    public class InstantEventListener : MonoBehaviour
    {
        public InstantEvent InstantEvent;
        public string Key;
        public UnityEvent listenerEvent;

        public void Invoke() => listenerEvent?.Invoke();

        private void OnEnable()
        {
            InstantEvent.Register(this);
        }

        private void OnDisable()
        {
            InstantEvent.UnRegister(this);
        }
    }
}
