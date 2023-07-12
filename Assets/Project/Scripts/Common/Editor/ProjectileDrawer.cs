using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Projectiles;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class ProjectileDrawer : OdinAttributeProcessor<Projectile>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "targetLayer")
            {
                attributes.Add(new PropertySpaceAttribute(0f, 10f));
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
    
    public class TrajectoryDrawer : OdinAttributeProcessor<Trajectory>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }
        
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "trajectoryType")
            {
                attributes.Add(new FoldoutGroupAttribute("TrajectoryTrait"));
            }
            
            if (member.Name == "speed")
            {
                attributes.Add(new FoldoutGroupAttribute("TrajectoryTrait"));
                attributes.Add(new PropertyRangeAttribute(1f, 100f));
            }
            
            if (member.Name == "distance")
            {
                attributes.Add(new FoldoutGroupAttribute("TrajectoryTrait"));
                attributes.Add(new PropertyRangeAttribute(1f, 100f));
            }
        }
    }
}
