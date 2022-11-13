#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

using Core.GameEvents.Listener;
using UnityEngine;
using UnityEngine.Events;

namespace Main.Manager.Save.Listener
{
    public class SaveEventListener : GameEventListener
    {
        [SerializeField] private UnityEvent loadEvent;

        public void SaveInvoke() => Invoke();
        public void LoadInvoke() => loadEvent?.Invoke();
        
        protected new void OnEnable()
        {
            LoadInvoke();
            targetEvent.Register(this);
        }

        protected new void OnDisable()
        {
            SaveInvoke();
            targetEvent.Unregister(this);
        }

#if UNITY_EDITOR
        private void EditorSetUp()
        {
            if (targetEvent is null)
                Finder.TryGetObject("Assets/Project/Data/GameEvent/Save", "SaveEvent", out targetEvent);
        }
        
        public class SaveEventListenerDrawer : OdinAttributeProcessor<SaveEventListener>
        {
            public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
            {
                if (member.Name == "targetEvent")
                {
                    attributes.Add(new LabelTextAttribute("Global Save Event"));
                }
            
                if (member.Name == "priority")
                {
                    attributes.Add(new HideInInspector());
                }
            
                if (member.Name == "response")
                {
                    attributes.Add(new LabelTextAttribute("Save Event"));
                }
            
                if (member.Name == "EditorSetUp")
                {
                    attributes.Add(new OnInspectorInitAttribute());
                }
            }
        }
#endif
    }
}
