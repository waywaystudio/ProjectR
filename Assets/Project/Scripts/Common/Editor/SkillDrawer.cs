using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Effects;
using Common.Execution;
using Common.Execution.Fires;
using Common.Execution.Hits;
using Common.Skills;
using Common.TargetSystem;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine;

namespace Common.Editor
{
    public class SkillDrawer : OdinAttributeProcessor<SkillComponent>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "icon")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(-1f));
                attributes.Add(new HorizontalGroupAttribute("CommonProperty", 0.25f));
                attributes.Add(new PreviewFieldAttribute(80f, ObjectFieldAlignment.Left));
            }
            
            if (member.Name == "dataIndex")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new VerticalGroupAttribute("CommonProperty/Fields"));
            }
            
            if (member.Name == "skillType")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new VerticalGroupAttribute("CommonProperty/Fields"));
                attributes.Add(new EnumPagingAttribute());
            }
            
            if (member.Name == "behaviourMask")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new VerticalGroupAttribute("CommonProperty/Fields"));
            }
            
            if (member.Name == "priority")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyRangeAttribute(0, 100));
                attributes.Add(new VerticalGroupAttribute("CommonProperty/Fields"));
            }
            
            if (member.Name == "description")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new DisplayAsStringAttribute(true, TextAlignment.Left, true));
                attributes.Add(new VerticalGroupAttribute("CommonProperty/Fields"));
            }
            
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
                attributes.Add(new PropertySpaceAttribute(0f, 10f));
            }
        }
    }
    
    public class SkillAnimationTraitDrawer : OdinAttributeProcessor<SkillAnimationTrait>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "animationKey")
            {
                attributes.Add(new FoldoutGroupAttribute("AnimationTrait"));
            }
            
            if (member.Name == "castCompleteAnimationKey")
            {
                attributes.Add(new FoldoutGroupAttribute("AnimationTrait"));
            }
            
            if (member.Name == "isLoop")
            {
                attributes.Add(new FoldoutGroupAttribute("AnimationTrait"));
            }
            
            if (member.Name == "hasExecuteEvent")
            {
                attributes.Add(new FoldoutGroupAttribute("AnimationTrait"));
            }
            
            if (member.Name == "animationPlaySpeed")
            {
                attributes.Add(new FoldoutGroupAttribute("AnimationTrait"));
                attributes.Add(new PropertyRangeAttribute(0, 10));
            }

            if (member.Name == "callbackSection")
            {
                attributes.Add(new FoldoutGroupAttribute("AnimationTrait"));
                attributes.Add(new EnumPagingAttribute());
            }
        }
    }
    
    public class SkillCoolTimerDrawer : OdinAttributeProcessor<SkillCoolTimer>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "duration")
            {
                attributes.Add(new PropertyRangeAttribute(0, 180));
                attributes.Add(new FoldoutGroupAttribute("CoolTimeTrait"));
            }
            
            if (member.Name == "isIncrease")
            {
                attributes.Add(new FoldoutGroupAttribute("CoolTimeTrait"));
            }
            
            if (member.Name == "invokeSection")
            {
                attributes.Add(new FoldoutGroupAttribute("CoolTimeTrait"));
            }
        }
    }
    
    public class SkillCastTimerDrawer : OdinAttributeProcessor<SkillCastTimer>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "duration")
            {
                attributes.Add(new PropertyRangeAttribute(0, 10));
                attributes.Add(new FoldoutGroupAttribute("CastTimeTrait"));
            }
            
            if (member.Name == "isIncrease")
            {
                attributes.Add(new FoldoutGroupAttribute("CastTimeTrait"));
            }
            
            if (member.Name == "callbackSection")
            {
                attributes.Add(new FoldoutGroupAttribute("CastTimeTrait"));
            }
        }
    }
    
    public class SkillCostDrawer : OdinAttributeProcessor<SkillCost>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "cost")
            {
                attributes.Add(new PropertyRangeAttribute(-100, 100));
                attributes.Add(new FoldoutGroupAttribute("CostTrait"));
            }
            
            if (member.Name == "paySection")
            {
                attributes.Add(new FoldoutGroupAttribute("CostTrait"));
            }
        }
    }
    
    public class HitExecutorDrawer : OdinAttributeProcessor<HitExecutor>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "hitExecutionList")
            {
                attributes.Add(new HideIfAttribute("@this.hitExecutionList.Count == 0"));
                attributes.Add(new ListDrawerSettingsAttribute()
                {
                    IsReadOnly = true, 
                    ShowFoldout = true,
                });
            }
        }
    }
    
    public class HitExecutionDrawer : OdinAttributeProcessor<HitExecution>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlineEditorAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "group")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new EnumPagingAttribute());
            }
        }
    }
    
    public class FireExecutorDrawer : OdinAttributeProcessor<FireExecutor>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "fireExecutionList")
            {
                attributes.Add(new HideIfAttribute("@this.fireExecutionList.Count == 0"));
                attributes.Add(new ListDrawerSettingsAttribute()
                {
                    IsReadOnly  = true, 
                    ShowFoldout = true,
                });
            }
        }
    }
    
    public class FireExecutionDrawer : OdinAttributeProcessor<FireExecution>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlineEditorAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "group")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new EnumPagingAttribute());
            }
        }
    }

    public class TakerDetectorDrawer : OdinAttributeProcessor<TakerDetector>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "sortingType")
            {
                attributes.Add(new EnumPagingAttribute());
                attributes.Add(new FoldoutGroupAttribute("DetectorTrait"));
            }
            
            if (member.Name == "maxBufferCount")
            {
                attributes.Add(new PropertyRangeAttribute(1, 64));
                attributes.Add(new FoldoutGroupAttribute("DetectorTrait"));
            }
            
            if (member.Name == "sizeVector")
            {
                attributes.Add(new FoldoutGroupAttribute("DetectorTrait"));
            }
            
            if (member.Name == "targetLayer")
            {
                attributes.Add(new FoldoutGroupAttribute("DetectorTrait"));
            }
        }
    }
}
