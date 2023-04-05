#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class StatDrawer : OdinAttributeProcessor<Stat>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
            attributes.Add(new HideLabelAttribute());
        }
    
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "statType")
            {
                attributes.Add(new PropertyOrderAttribute(1f));
                attributes.Add(new HorizontalGroupAttribute("StatValue", 0.25f));
                attributes.Add(new HideLabelAttribute());
            }
            
            if (member.Name == "value")
            {
                attributes.Add(new PropertyOrderAttribute(2f));
                attributes.Add(new HorizontalGroupAttribute("StatValue"));
                attributes.Add(new HideLabelAttribute());
            }
            
            if (member.Name == "applyType")
            {
                attributes.Add(new PropertyOrderAttribute(3f));
                attributes.Add(new HorizontalGroupAttribute("StatValue", 0.3f));
                attributes.Add(new HideLabelAttribute());
            }
        }
    }
    
    public class SpecDrawer : OdinAttributeProcessor<Spec>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
            attributes.Add(new HideLabelAttribute());
        }
    
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "statList")
            {
                attributes.Add(new TableListAttribute
                {
                    AlwaysExpanded = true,
                });
                attributes.Add(new HideLabelAttribute());
            }
        }
    }
    
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