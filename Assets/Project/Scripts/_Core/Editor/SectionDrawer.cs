using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class SectionDrawer : OdinAttributeProcessor<Section>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "sectionType")
            {
                attributes.Add(new HideReferenceObjectPickerAttribute());
                attributes.Add(new HideLabelAttribute());
            }

            if (member.Name == "ActionTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
            }
        
        }
    }
}
