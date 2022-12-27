using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Character.Operation.Combat.Entity;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Common.Character.Operation.Combat.Editor
{
    public class CombatDrawer : OdinAttributeProcessor<Combating>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "SkillTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    public class BaseEntryDrawer : OdinAttributeProcessor<BaseEntity>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "skill")
            {
                attributes.Add(new HideInInspector());
            }
        }
    }
    public class CastingEntityDrawer : OdinAttributeProcessor<CastingEntity>
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
    public class BaseSkillDrawer : OdinAttributeProcessor<BaseSkill>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "id")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "skillName")
            {
                attributes.Add(new DisplayAsStringAttribute());
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
            
            if (member.Name == "GetDataFromDB")
            {
                attributes.Add(new HorizontalGroupAttribute("Editor Functions"));
                attributes.Add(new PropertySpaceAttribute(15, 0));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                               {
                                       Icon = SdfIconType.ArrowRepeat,
                               });
            }
            
            if (member.Name == "ShowDB")
            {
                attributes.Add(new HorizontalGroupAttribute("Editor Functions"));
                attributes.Add(new PropertySpaceAttribute(15, 0));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                               {
                                       Icon = SdfIconType.ListColumnsReverse,
                               });
            }
        }
    }
    
}
