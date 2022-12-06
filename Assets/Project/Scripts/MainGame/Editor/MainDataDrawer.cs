using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace MainGame.Editor
{
    public class MainDataDrawer : OdinAttributeProcessor
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "dataObjectList")
            {
                attributes.Add(new ListDrawerSettingsAttribute
                {
                   Expanded = true,
                   HideAddButton = true,
                   HideRemoveButton = true,
                   IsReadOnly = true,
                });
            }
            
            if (member.Name == "GetAllData")
            {
                attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
            }
            
            if (member.Name == "OpenSpreadSheetPanel")
            {
                attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
            }
        }
    }
}
