using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Character.Editor
{
    public class CharacterBehaviourDrawer : OdinAttributeProcessor<CharacterBehaviour>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Initialize")
            {
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                        Icon = SdfIconType.ArrowRepeat,
                        Stretch = false,
                        ButtonHeight = 40,
                });
            }
            
            if (member.Name == "id")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }
            if (member.Name == "combatClass")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }
            if (member.Name == "StatTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
