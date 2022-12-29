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
            if (member.Name == "dataList")
            {
                attributes.Add(new PropertySpaceAttribute(0, 20f));
                attributes.Add(new ListDrawerSettingsAttribute
                {
                   Expanded = true,
                   HideAddButton = true,
                   HideRemoveButton = true,
                   IsReadOnly = true,
                });
            }
            
            if (member.Name == "dataScriptPath")
            {
                attributes.Add(new FolderPathAttribute());
            }
            
            if (member.Name == "dataObjectPath")
            {
                attributes.Add(new PropertySpaceAttribute(0, 20f));
                attributes.Add(new FolderPathAttribute());
            }

            if (member.Name == "SetUp")
            {
                attributes.Add(new HorizontalGroupAttribute("Horizontal"));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                    Icon = SdfIconType.Save,
                });
            }
            
            if (member.Name == "OpenSpreadSheetPanel")
            {
                attributes.Add(new HorizontalGroupAttribute("Horizontal"));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                    Icon = SdfIconType.FileSpreadsheet,
                });
            }
        }
    }
}
