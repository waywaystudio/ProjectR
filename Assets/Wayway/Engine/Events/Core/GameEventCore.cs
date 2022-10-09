using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wayway.Engine.Events.Core
{
    public class GameEventCore : ScriptableObject
    {
        [SerializeField] protected List<GameEventListenerCore> listenerList;

        public void Register(GameEventListenerCore listener)
        {
            if (!listenerList.Contains(listener))
            {
                listenerList.Add(listener);
                listenerList = listenerList.OrderByDescending(x => x.Priority)
                                           .ToList();
            }
        }
        
        public void UnRegister(GameEventListenerCore listener)
        {
            if (listenerList.Contains(listener)) 
                listenerList.Remove(listener);
        }
        
#if UNITY_EDITOR
        #region - editor Functions :: Get Subscribers
        [SerializeField] protected List<GameEventListenerCore> subscriberList;

        public void ShowListener()
        {
            var listeners = FindObjectsOfType(typeof(GameEventListenerCore)) as GameEventListenerCore[];

            var subscriberArray = from listener in listeners
                where listener.TargetEventList.Contains(this)
                select listener;

            subscriberArray = subscriberArray.OrderBy(x => x.Priority);
            subscriberList = subscriberArray.ToList();
        }
        #endregion
#endif
    }
}


