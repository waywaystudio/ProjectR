#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class DynamicStatValueDrawer : OdinAttributeProcessor<DynamicStat>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "value")
            {
                attributes.Add(new PropertyOrderAttribute(2f));
                attributes.Add(new HideLabelAttribute());
            }
        }
    }
    
    public class AliveValueDrawer : OdinAttributeProcessor<AliveValue>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "value")
            {
                attributes.Add(new PropertyOrderAttribute(2f));
                attributes.Add(new HorizontalGroupAttribute("StatValue"));
                attributes.Add(new HideLabelAttribute());
            }
        }
    }

#endif
}