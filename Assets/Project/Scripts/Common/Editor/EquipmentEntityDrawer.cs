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
            attributes.Add(new HideLabelAttribute());
            
            if (member.Name == "Icon")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new PropertyOrderAttribute(-1f));
                attributes.Add(new HorizontalGroupAttribute("EquipmentEntity", 0.1f));
                attributes.Add(new PreviewFieldAttribute(ObjectFieldAlignment.Center));
            }
            
            if (member.Name == "DataIndex")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new VerticalGroupAttribute("EquipmentEntity/Entity"));
            }
            if (member.Name == "ItemName")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new VerticalGroupAttribute("EquipmentEntity/Entity"));
            }
            if (member.Name == "ConstStatSpec")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new VerticalGroupAttribute("EquipmentEntity/Entity"));
            }
        }
    }
}
