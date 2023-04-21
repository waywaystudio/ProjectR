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
            /*
             * SpreadSheetData
             */ 
            if (member.Name == "sheetDataList")
            {
                attributes.Add(new TitleGroupAttribute("SheetData", "dataObject and configurations"));
                attributes.Add(new PropertySpaceAttribute(0, 20f));
                attributes.Add(new ListDrawerSettingsAttribute
                {
                   Expanded = true,
                   HideAddButton = true,
                   HideRemoveButton = true,
                   IsReadOnly = true,
                });
            }

            /*
             * PrefabData
             */
            if (member.Name.Contains("PrefabData"))
            {
                attributes.Add(new TitleGroupAttribute("PrefabData", "ScriptableObjects"));
            }

            /*
             * Paths
             */
            if (member.Name.Contains("Path"))
            {
                attributes.Add(new TitleGroupAttribute("Path", "Object Locate Path"));
                attributes.Add(new FolderPathAttribute());
            }

            /*
             * Functions
             */
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
