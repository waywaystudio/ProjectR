using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Equipments;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Common.Editor
{
    public class EquipmentEntityDrawer : OdinAttributeProcessor<EquipmentEntity>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
            attributes.Add(new HideReferenceObjectPickerAttribute());
            attributes.Add(new HideLabelAttribute());
        }
        
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "dataIndex")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new VerticalGroupAttribute("EquipmentEntity/Entity"));
            }
            if (member.Name == "availableClassType")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new VerticalGroupAttribute("EquipmentEntity/Entity"));
            }
            if (member.Name == "itemName")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new VerticalGroupAttribute("EquipmentEntity/Entity"));
            }
            if (member.Name == "icon")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(-1f));
                attributes.Add(new HorizontalGroupAttribute("EquipmentEntity", 0.15f));
                attributes.Add(new PreviewFieldAttribute(ObjectFieldAlignment.Center));
            }
            if (member.Name == "constSpec")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new HorizontalGroupAttribute("EquipmentEntity", 0.5f));
            }
            
        }
    }
}
