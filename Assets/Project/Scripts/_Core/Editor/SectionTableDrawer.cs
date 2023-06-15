using System;
using System.Collections.Generic;
using System.Reflection;
using Sequences;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class SectionTableDrawer : OdinAttributeProcessor<SectionTable>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "hideKey")
            {
                attributes.Add(new FoldoutGroupAttribute("Edit", false));
            }
            
            if (member.Name == "AddSection")
            {
                attributes.Add(new FoldoutGroupAttribute("Edit", false));
                attributes.Add(new HorizontalGroupAttribute("Edit/Button"));
                attributes.Add(new HideInPlayModeAttribute());
                attributes.Add(new ButtonAttribute
                {
                    Style = ButtonStyle.FoldoutButton,
                    Icon = SdfIconType.Plus,
                    ButtonHeight = 24,
                });
            }
            
            if (member.Name == "RemoveSection")
            {
                attributes.Add(new FoldoutGroupAttribute("Edit"));
                attributes.Add(new HorizontalGroupAttribute("Edit/Button"));
                attributes.Add(new HideInPlayModeAttribute());
                attributes.Add(new ButtonAttribute
                {
                    Style        = ButtonStyle.FoldoutButton,
                    Icon         = SdfIconType.FileMinus,
                    ButtonHeight = 24,
                });
            }
        }
    }
}
