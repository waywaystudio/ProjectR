using System;
using System.Collections.Generic;
using System.Reflection;
using Common.StatusEffects;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class StatusEffectDrawer : OdinAttributeProcessor<StatusEffect>
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
            
            if (member.Name == "statusCode")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new VerticalGroupAttribute("CommonProperty/Fields"));
            }
            
            if (member.Name == "type")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new VerticalGroupAttribute("CommonProperty/Fields"));
            }

            if (member.Name == "duration")
            {
                // attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyRangeAttribute(0, 60));
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
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
            }
        }
    }
}
