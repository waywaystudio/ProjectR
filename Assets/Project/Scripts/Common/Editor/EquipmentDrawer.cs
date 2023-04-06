using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Equipments;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Common.Editor
{
    public class EquipmentDrawer : OdinAttributeProcessor<Equipment>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "title")
            {
                attributes.Add(new HideInInspector());
            }
        }
    }
    
    public class EquipmentInfoDrawer : OdinAttributeProcessor<EquipmentInfo>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "dataCode")
            {
                attributes.Add(new HorizontalGroupAttribute("EquipmentInfo"));
                attributes.Add(new HideLabelAttribute());
            }
            
            if (member.Name == "enchantLevel")
            {
                attributes.Add(new HorizontalGroupAttribute("EquipmentInfo"));
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyRangeAttribute(0, 10));
            }
        }
    }
}
