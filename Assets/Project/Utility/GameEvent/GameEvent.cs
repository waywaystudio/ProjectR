using System.Collections.Generic;
using System.Linq;
using GameEvent.Listener;
using UnityEngine;

namespace GameEvent
{
    public class GameEvent : ScriptableObject
    {
        [SerializeField] protected List<GameEventListener> listenerList;
        
        protected List<GameEventListener> ListenerList 
        {
            get => listenerList;
            set => listenerList = value;
        }
        
        public void Register(GameEventListener listener)
        {
            if (!ListenerList.Contains(listener))
            {
                ListenerList.Add(listener);
                ListenerList = ListenerList.OrderByDescending(x => x.Priority)
                                           .ToList();
            }
        }
        
        public void Unregister(GameEventListener listener)
        {
            if (ListenerList.Contains(listener)) 
                ListenerList.Remove(listener);
        }

        public void Invoke() => ListenerList.ForEach(x => x.Invoke());
        
#if UNITY_EDITOR
        #region - editor Functions :: Get Subscribers
        [SerializeField] protected List<GameEventListener> subscriberList;

        public void ShowListener()
        {
            if (FindObjectsOfType(typeof(GameEventListener)) is not GameEventListener[] allOfListeners) return; 
            
            var subscriber = allOfListeners.Where(x => x.TargetEvent == this);

            subscriberList = subscriber.OrderBy(x => x.Priority)
                                       .ToList();
        }
        #endregion
#endif
    }
}
