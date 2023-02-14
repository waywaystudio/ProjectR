using System;
using System.Collections.Generic;
using System.Reflection;
using Character.Targeting;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Character.Editor
{
    public class SearchingDrawer : OdinAttributeProcessor<Searching>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "AdventurerList")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "MonsterList")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }

            if (member.Name == "LookTarget")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
