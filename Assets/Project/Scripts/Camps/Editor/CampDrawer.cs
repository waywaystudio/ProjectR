using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Camps.Editor
{
    public class CampDrawer : OdinAttributeProcessor<Camp>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "growMaterialInventory")
            {
                attributes.Add(new TitleGroupAttribute("Inventory", "grow, vice and so on"));
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new HideReferenceObjectPickerAttribute());
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
