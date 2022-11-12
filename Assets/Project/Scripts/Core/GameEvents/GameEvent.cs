#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif
using System;
using Core.GameEvents.Listener;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.GameEvents
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
        #region - editor Functions & Attributes
        [SerializeField] protected List<GameEventListener> subscriberList;

        public void ShowListener()
        {
            if (FindObjectsOfType(typeof(GameEventListener)) is not GameEventListener[] allOfListeners) return; 
            
            var subscriber = allOfListeners.Where(x => x.TargetEvent == this);

            subscriberList = subscriber.OrderBy(x => x.Priority)
                                       .ToList();
        }

        public class GameEventDrawer : OdinAttributeProcessor<GameEvent>
        {
            public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
            {
                switch (member.Name)
                {
                    case "subscriberList":
                        attributes.Add(new ReadOnlyAttribute());
                        break;
                    case "ShowListener":
                        attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                        break;
                }
            }
        }
        
        public class GameEventDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : GameEvent<T1>
        {
            public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
            {
                switch (member.Name)
                {
                    case "subscriberList":
                        attributes.Add(new ReadOnlyAttribute());
                        break;
                    case "ShowListener":
                        attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                        break;
                }
            }
        }
        
        public class GameEventDrawer<T0, T1, T2> : OdinAttributeProcessor<T0> where T0 : GameEvent<T1, T2>
        {
            public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
            {
                switch (member.Name)
                {
                    case "subscriberList":
                        attributes.Add(new ReadOnlyAttribute());
                        break;
                    case "ShowListener":
                        attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                        break;
                }
            }
        }
        #endregion
#endif
    }
}
