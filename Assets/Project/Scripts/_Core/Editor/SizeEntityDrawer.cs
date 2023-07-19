using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class SizeEntityDrawer : OdinAttributeProcessor<SizeEntity> 
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "x")
            {
                attributes.Add(new LabelTextAttribute("PivotRange from Provider"));
            }
            
            if (member.Name == "y")
            {
                attributes.Add(new LabelTextAttribute("Affect Range or Height"));
            }
            
            if (member.Name == "z")
            {
                attributes.Add(new LabelTextAttribute("Angle or Width"));
            }
        }
    }
}
