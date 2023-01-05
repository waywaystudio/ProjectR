using System;
using System.Collections.Generic;
using System.Reflection;
using Character.Combat.Entities;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Character.Editor
{
    public class DamageEntityDrawer : OdinAttributeProcessor<DamageEntity>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "StatTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class HealEntityDrawer : OdinAttributeProcessor<HealEntity>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "StatTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class TargetEntityDrawer : OdinAttributeProcessor<TargetEntity>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "searchedList")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "combatTaker")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
