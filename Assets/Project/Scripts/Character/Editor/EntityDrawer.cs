using System;
using System.Collections.Generic;
using System.Reflection;
using Character.Combat.Skill.Modules;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Character.Editor
{
    public class DamageEntityDrawer : OdinAttributeProcessor<DamageSkill>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "StatTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class HealEntityDrawer : OdinAttributeProcessor<HealSkill>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "StatTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class TargetEntityDrawer : OdinAttributeProcessor<TargetSkill>
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
    
    public class StatusEffectEntityDrawer : OdinAttributeProcessor<StatusEffectSkill>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "statusEffectPool")
            {
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    Expanded = true,
                    HideRemoveButton = true,
                    IsReadOnly = true,
                    HideAddButton = true,
                });
            }
            
            if (member.Name == "combatTaker")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
