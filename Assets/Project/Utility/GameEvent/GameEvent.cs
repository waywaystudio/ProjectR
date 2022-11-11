using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wayway.Engine.Events
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
            if (FindObjectsOfType(typeof(GameEventListener)) is GameEventListener[] allOfListeners)
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
