using UnityEngine;
using UnityEngine.Events;

namespace Wayway.Engine.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] protected GameEvent targetEvent;
        [SerializeField] protected int priority = 5;
        [SerializeField] private UnityEvent response;

        public float Priority => priority;
        public GameEvent TargetEvent => targetEvent;
        
        public void Invoke() => response?.Invoke();
        
        protected void OnEnable() => TargetEvent.Register(this);
        protected void OnDisable() => TargetEvent.Unregister(this);
    }

    public class GameEventListener<T0> : MonoBehaviour
    {
        [SerializeField] protected GameEvent<T0> targetEvent;
        [SerializeField] protected int priority = 5;
        [SerializeField] private UnityEvent<T0> response;
        
        public float Priority => priority;
        public GameEvent<T0> TargetEvent => targetEvent;

        public void Invoke(T0 value) => response?.Invoke(value);
        
        protected void OnEnable() => TargetEvent.Register(this);
        protected void OnDisable() => TargetEvent.Unregister(this);
    }

    public class GameEventListener<T0, T1> : MonoBehaviour
    {
        [SerializeField] protected GameEvent<T0, T1> targetEvent;
        [SerializeField] protected int priority = 5;
        [SerializeField] private UnityEvent<T0, T1> response;
        
        public float Priority => priority;
        public GameEvent<T0, T1> TargetEvent => targetEvent;

        public void Invoke(T0 value1, T1 value2) => response?.Invoke(value1, value2);
        
        protected void OnEnable() => TargetEvent.Register(this);
        protected void OnDisable() => TargetEvent.Unregister(this);
    }
}
