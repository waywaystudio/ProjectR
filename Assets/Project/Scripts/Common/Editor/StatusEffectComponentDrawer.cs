using System;
using System.Collections.Generic;
using System.Reflection;
using Common.StatusEffect;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class StatusEffectComponentDrawer : OdinAttributeProcessor<StatusEffectComponent>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "OnActivated")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "OnCanceled")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "OnCompleted")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "OnEnded")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
