using System;
using System.Collections.Generic;
using System.Reflection;
using Character.Villains;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Character.Editor
{
    public class PhaseBehaviourDrawer : OdinAttributeProcessor<PhaseBehaviours>
    {
        // public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        // {
        //     attributes.Add(new HideLabelAttribute());
        //     attributes.Add(new HideReferenceObjectPickerAttribute());
        // }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name.Contains("phase"))
            {
                var tabName = member.Name;
                
                attributes.Add(new TabGroupAttribute("PhaseGroup", tabName));
            }
        }
    }
}
