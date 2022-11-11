#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Wayway.Engine.Events.Editor
{
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
    
    public class GameEventListenerDrawer : OdinAttributeProcessor<GameEventListener>
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
    
    public class GameEventListenerDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : GameEventListener<T1>
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
}
#endif


