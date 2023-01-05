using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Character.Data;
using Character.Data.BaseStats;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Character.Editor
{
    public class EquipmentItemDrawer : OdinAttributeProcessor<EquipmentItem>  
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "SetStats")
            {
                attributes.Add(new ButtonAttribute(ButtonSizes.Medium)
                {
                    Icon = SdfIconType.ArrowRepeat,
                    Stretch = false
                });
            }
        }
    }
}
