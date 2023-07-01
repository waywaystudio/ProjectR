#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector.Editor;
#endif

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents.Listener
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] protected GameEvent targetEvent;
        [SerializeField] protected int priority = 5;
        [SerializeField] protected UnityEvent response;

        public float Priority => priority;
        public GameEvent TargetEvent => targetEvent;
        
        public void Invoke() => response?.Invoke();
        
        protected void OnEnable() => TargetEvent.Register(this);
        protected void OnDisable() => TargetEvent.Unregister(this);


#if UNITY_EDITOR
        private void AutoRegister()
        {
            if (targetEvent == null)
            {
                response.ClearAllUnityEventInEditor();
                return;
            }
            
            var behaviours = GetComponents<MonoBehaviour>();

            foreach (var behaviour in behaviours)
            {
                var method = behaviour.GetType().GetMethod(targetEvent.name,BindingFlags.Public | BindingFlags.Instance);

                if (method == null) continue;
                
                response.ClearAllUnityEventInEditor();
                response.AddPersistantListenerInEditor(behaviour, targetEvent.name);
            }
        }
#endif
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
        
        
#if UNITY_EDITOR
        private void AutoRegister()
        {
            if (targetEvent == null)
            {
                response.ClearAllUnityEventInEditor();
                return;
            }
            
            var behaviours = GetComponents<MonoBehaviour>();

            foreach (var behaviour in behaviours)
            {
                var method = behaviour.GetType().GetMethod(targetEvent.name,BindingFlags.Public | BindingFlags.Instance);

                if (method == null) continue;
                
                response.ClearAllUnityEventInEditor();
                response.AddPersistantListenerInEditor(behaviour, targetEvent.name);
            }
        }
#endif
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

#if UNITY_EDITOR && ODIN_INSPECTOR
    public class GameEventListenerDrawer : OdinAttributeProcessor<GameEventListener>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "targetEvent")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new HorizontalGroupAttribute
                {
                    GroupName = "EventField",
                });
            }
            
            if (member.Name == "priority")
            {
                attributes.Add(new PropertyRangeAttribute(0, 10));
                attributes.Add(new HideLabelAttribute());
            }
            
            if (member.Name == "AutoRegister")
            {
                attributes.Add(new PropertyOrderAttribute(-1f));
                attributes.Add(new ButtonAttribute
                {
                    Icon = SdfIconType.ArrowRepeat
                });
                attributes.Add(new HorizontalGroupAttribute
                {
                    GroupName = "EventField",
                    Width     = 0.1f,
                });
            }
        }
    }
    
    public class GameEventListenerDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : GameEventListener<T1>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "targetEvent")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new HorizontalGroupAttribute
                {
                    GroupName = "EventField",
                });
            }
            
            if (member.Name == "priority")
            {
                attributes.Add(new PropertyRangeAttribute(0, 10));
                attributes.Add(new HideLabelAttribute());
            }
            
            if (member.Name == "AutoRegister")
            {
                attributes.Add(new PropertyOrderAttribute(-1f));
                attributes.Add(new ButtonAttribute
                {
                    Icon = SdfIconType.ArrowRepeat
                });
                attributes.Add(new HorizontalGroupAttribute
                {
                    GroupName = "EventField",
                    Width     = 0.1f,
                });
            }
        }
    }

    public class GameEventListenerDrawer<T0, T1, T2> : OdinAttributeProcessor<T0> where T0 : GameEventListener<T1, T2>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "priority")
            {
                attributes.Add(new PropertyRangeAttribute(0, 10));
                attributes.Add(new HideLabelAttribute());
            }
        }
    }
#endif
}
