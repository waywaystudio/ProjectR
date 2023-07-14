using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Traps;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Common.Editor
{
    public class TrapDrawer : OdinAttributeProcessor<Trap>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "hitExecutor")
            {
                attributes.Add(new TitleGroupAttribute("Executors", "Hit and Fire"));
            }
            
            if (member.Name == "fireExecutor")
            {
                attributes.Add(new TitleGroupAttribute("Executors"));
                attributes.Add(new PropertySpaceAttribute(0f, 10f));
            }
            
            if (member.Name == "effector")
            {
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
            }
        }
    }
}
