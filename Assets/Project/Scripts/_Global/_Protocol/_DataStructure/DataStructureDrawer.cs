#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class DataStructureDrawer : OdinAttributeProcessor<StatValue>
{
    public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
    {
        attributes.Add(new InlinePropertyAttribute());
        attributes.Add(new HideLabelAttribute());
    }

    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        if (member.Name == "statCode")
        {
            attributes.Add(new PropertyOrderAttribute(1f));
            attributes.Add(new HorizontalGroupAttribute("StatValue", 0.3f));
            attributes.Add(new HideLabelAttribute());
        }
            
        if (member.Name == "value")
        {
            attributes.Add(new PropertyOrderAttribute(2f));
            attributes.Add(new HorizontalGroupAttribute("StatValue"));
            attributes.Add(new HideLabelAttribute());
        }
    }
}
    
public class DynamicStatValueDrawer : OdinAttributeProcessor<DynamicStatValue>
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