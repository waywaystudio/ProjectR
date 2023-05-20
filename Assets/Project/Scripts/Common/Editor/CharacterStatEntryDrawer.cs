using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class CharacterStatEntryDrawer : OdinAttributeProcessor<CharacterStats>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Alive")
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
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "DeBuffTable")
            {
                attributes.Add(new PropertySpaceAttribute(15f, 0f));
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
