using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wayway.Engine.Events
{
    public class GameEvent<T0> : ScriptableObject
    {
        [SerializeField] protected List<GameEventListener<T0>> listenerList;
        
        protected List<GameEventListener<T0>> ListenerList 
        {
            get => listenerList;
            set => listenerList = value;
        }
        
        public void Register(GameEventListener<T0> listener)
        {
            if (!ListenerList.Contains(listener))
            {
                ListenerList.Add(listener);
                ListenerList = ListenerList.OrderByDescending(x => x.Priority)
                    .ToList();
            }
        }
        
        public void Unregister(GameEventListener<T0> listener)
        {
            if (ListenerList.Contains(listener)) 
                ListenerList.Remove(listener);
        }

        public void Invoke(T0 value) => ListenerList.ForEach(x => x.Invoke(value));
        
#if UNITY_EDITOR
        #region - editor Functions :: Get Subscribers
        [SerializeField] protected List<GameEventListener<T0>> subscriberList;

        public void ShowListener()
        {
            if (FindObjectsOfType(typeof(GameEventListener<T0>)) is GameEventListener<T0>[] allOfListeners)
            {
                var subscriber = allOfListeners.Where(x => x.TargetEvent == this);

                subscriber = subscriber.OrderBy(x => x.Priority);
                subscriber = subscriber.ToList();
            }
        }
        #endregion
#endif
    }
    
    public class GameEvent<T0, T1> : ScriptableObject
    {
        [SerializeField] protected List<GameEventListener<T0, T1>> listenerList;
        
        protected List<GameEventListener<T0, T1>> ListenerList 
        {
            get => listenerList;
            set => listenerList = value;
        }
        
        public void Register(GameEventListener<T0, T1> listener)
        {
            if (!ListenerList.Contains(listener))
            {
                ListenerList.Add(listener);
                ListenerList = ListenerList.OrderByDescending(x => x.Priority)
                    .ToList();
            }
        }
        
        public void Unregister(GameEventListener<T0, T1> listener)
        {
            if (ListenerList.Contains(listener)) 
                ListenerList.Remove(listener);
        }

        public void Invoke(T0 value1, T1 value2) => ListenerList.ForEach(x => x.Invoke(value1, value2));
        
#if UNITY_EDITOR
        #region - editor Functions :: Get Subscribers
        [SerializeField] protected List<GameEventListener<T0, T1>> subscriberList;

        public void ShowListener()
        {
            if (FindObjectsOfType(typeof(GameEventListener<T0, T1>)) is GameEventListener<T0, T1>[] allOfListeners)
            {
                var subscriber = allOfListeners.Where(x => x.TargetEvent == this);

                subscriber = subscriber.OrderBy(x => x.Priority);
                subscriber = subscriber.ToList();
            }
        }
        #endregion
#endif
    }
}