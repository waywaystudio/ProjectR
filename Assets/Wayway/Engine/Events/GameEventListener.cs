using UnityEngine;
using UnityEngine.Events;
using Wayway.Engine.Events.Core;

namespace Wayway.Engine.Events
{
    public class GameEventListener : GameEventListenerCore
    {
        [SerializeField] private UnityEvent response;

        public void Invoke() => response?.Invoke();
    }

    public class GameEventListener<T0> : GameEventListenerCore
    {
        [SerializeField] private UnityEvent<T0> response;

        public void Invoke(T0 value) => response?.Invoke(value);
    }

    public class GameEventListener<T0, T1> : GameEventListenerCore
    {
        [SerializeField] private UnityEvent<T0, T1> response;

        public void Invoke(T0 value1, T1 value2) => response?.Invoke(value1, value2);
    }
}
