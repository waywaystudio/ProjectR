using System;
using System.Collections.Generic;
using System.Reflection;
using Character.Combat;
using Character.Combat.Skill;
using Character.Combat.Skill.Modules;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Character.Editor
{
    public class CombatDrawer : OdinAttributeProcessor<CombatBehaviour>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "SkillTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class BaseEntityDrawer : OdinAttributeProcessor<SkillModule>
    {
        // public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        // {
        //     if (member.Name == "skill")
        //     {
        //         attributes.Add(new HideInInspector());
        //     }
        // }
    }
    
    public class CastingEntityDrawer : OdinAttributeProcessor<CastingSkill>
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

    public class CoolTimeEntityDrawer : OdinAttributeProcessor<CoolTimeSkill>
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

    public class BaseSkillDrawer : OdinAttributeProcessor<SkillObject>
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
            
            // if (member.Name == "SetUp")
            // {
            //     attributes.Add(new HorizontalGroupAttribute("Editor Functions"));
            //     attributes.Add(new PropertySpaceAttribute(15, 0));
            //     attributes.Add(new ButtonAttribute(ButtonSizes.Large)
            //                    {
            //                            Icon = SdfIconType.ArrowRepeat,
            //                    });
            // }
            
            if (member.Name == "ShowDB")
            {
                attributes.Add(new HorizontalGroupAttribute("CommonHorizontal"));
                // attributes.Add(new PropertySpaceAttribute(15, 0));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                               {
                                       Icon = SdfIconType.ListColumnsReverse,
                               });
            }
        }
    }
    
}
