using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class DatabaseDrawer : OdinAttributeProcessor<Database>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "sheetDataList")
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
            
            if (member.Name == "SheetDataTable")
            {
                // attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "iconPath")
            {
                attributes.Add(new FolderPathAttribute());
            }
            
            if (member.Name == "idCodePath")
            {
                attributes.Add(new FolderPathAttribute());
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
            
            
            
            if (member.Name == "OpenSpreadSheetPanel")
            {
                attributes.Add(new HorizontalGroupAttribute("CommonHorizontal"));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                    Icon = SdfIconType.FileSpreadsheet,
                });
            }
            
            if (member.Name == "GenerateIDCode")
            {
                attributes.Add(new HorizontalGroupAttribute("CommonHorizontal"));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                    Icon = SdfIconType.Gear,
                });
            }
        }
    }
}