using System;
using System.Collections.Generic;
using System.Reflection;
using Character.Data;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Character.Editor
{
    public class DynamicStatEntryDrawer : OdinAttributeProcessor<DynamicStatEntry>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "IsAlive")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "Hp")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "Resource")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "Shield")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "BuffTable")
            {
                attributes.Add(new PropertySpaceAttribute(15f, 0f));
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "DeBuffTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
