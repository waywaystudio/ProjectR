using System;
using System.Collections.Generic;
using System.Reflection;
using Camps.Inventories;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Camps.Editor
{
    public class InventoryDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : Inventory<T1>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Table")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new PropertyOrderAttribute(2f));
                attributes.Add(new DictionaryDrawerSettings()
                {
                    IsReadOnly = true
                });
            }
        }
    }
    
    public class GrowMaterialInventoryDrawer : OdinAttributeProcessor<GrowMaterialInventory>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "AddAll100Material")
            {
                attributes.Add(new ButtonAttribute
                {
                    ButtonHeight = 40,
                    Icon         = SdfIconType.Plus,
                    IconAlignment = IconAlignment.LeftEdge,
                    Stretch = false,
                });
                attributes.Add(new PropertyOrderAttribute(2f));
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
                attributes.Add(new DictionaryDrawerSettings()
                {
                    IsReadOnly = true
                });
            }
        }
    }
}
