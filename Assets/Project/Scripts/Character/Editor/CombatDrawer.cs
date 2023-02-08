using System;
using System.Collections.Generic;
using System.Reflection;
using Character.Combat;
using Character.Combat.Skill;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Character.Editor
{
    public class CombatDrawer : OdinAttributeProcessor<OldSkillBehaviour>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "SkillTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class ModuleDrawer : OdinAttributeProcessor<OldCombatModule>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "CombatObject")
            {
                // attributes.Add(new HideInInspector());
            }
        }
    }
    
    public class CastingModuleDrawer : OdinAttributeProcessor<OldCastingModule>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "CastingTime")
            {
                // attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "RemainTimer")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }

    public class CoolTimeModuleDrawer : OdinAttributeProcessor<CoolTimeModule>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "CastingTime")
            {
                // attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "RemainTimer")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class TargetModuleDrawer : OdinAttributeProcessor<TargetModule>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "targetLayer")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class CombatObjectDrawer : OdinAttributeProcessor<CombatObject>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "actionCode")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }
        }
    }

    public class SkillObjectDrawer : OdinAttributeProcessor<SkillObject>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "skillName")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }
            if (member.Name == "icon")
            {
                attributes.Add(new PreviewFieldAttribute(ObjectFieldAlignment.Left));
            }
            
            if (member.Name == "SkillData")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "OnStarted")
            {
                attributes.Add(new ShowIfAttribute("@UnityEngine.Application.isPlaying"));
            }
            
            if (member.Name == "OnInterrupted")
            {
                attributes.Add(new ShowIfAttribute("@UnityEngine.Application.isPlaying"));
            }
            
            if (member.Name == "OnCompleted")
            {
                attributes.Add(new ShowIfAttribute("@UnityEngine.Application.isPlaying"));
            }
            
            if (member.Name == "ShowDB")
            {
                attributes.Add(new HorizontalGroupAttribute("CommonHorizontal"));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                               {
                                       Icon = SdfIconType.ListColumnsReverse,
                               });
            }
        }
    }
    
}
