using System;
using System.Collections.Generic;
using System.Reflection;
using Character.Combat.StatusEffects;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Character.Editor
{
    public class StatusEffectDrawer : OdinAttributeProcessor<StatusEffectBehaviour>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "id")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "isBuff")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }

            if (member.Name == "icon")
            {
                attributes.Add(new PreviewFieldAttribute(ObjectFieldAlignment.Left));
            }
            
            if (member.Name == "TargetTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new ReadOnlyAttribute());
            }
        }
    }
}
