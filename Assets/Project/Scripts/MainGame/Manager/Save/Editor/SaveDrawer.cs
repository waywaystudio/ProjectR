using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable UnusedType.Global

namespace MainGame.Manager.Save.Editor
{
    public class SaveInfoDrawer : OdinAttributeProcessor<SaveInfo>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "saveName")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new DisplayAsStringAttribute());
                attributes.Add(new HorizontalGroupAttribute("Main", 180f));
                attributes.Add(new VerticalGroupAttribute("Main/Left"));
            }
            
            if (member.Name == "saveTime")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new DisplayAsStringAttribute());
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Left"));
            }
            
            if (member.Name == "lastSceneName")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new DisplayAsStringAttribute());
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Left"));
            }
            
            if (member.Name == "Save")
            {
                attributes.Add(new HideIfAttribute("@this.saveName == \"_autoSaveFile\""));
                attributes.Add(new ButtonAttribute("Save to slot"));
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Right"));
            }
            
            if (member.Name == "Load")
            {
                attributes.Add(new ButtonAttribute("Load from slot"));
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Right"));
            }
            
            if (member.Name == "Delete")
            {
                attributes.Add(new HideIfAttribute("@this.saveName == \"_autoSaveFile\""));
                attributes.Add(new ButtonAttribute());
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Right"));
            }
        }
    }
    
    public class SaveManagerDrawer : OdinAttributeProcessor<SaveManager>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "saveEvent")
            {
                attributes.Add(new TitleGroupAttribute("Core Event", "Save & Scene"));
            }
            
            if (member.Name == "sceneChangeEvent")
            {
                attributes.Add(new TitleGroupAttribute("Core Event"));
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
            }
            
            if (member.Name == "saveInfoList")
            {
                attributes.Add(new TitleGroupAttribute("Current SaveFiles", "Data/SaveFile"));
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    Expanded = true,
                    IsReadOnly = true,
                });
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
            }
            
            if (member.Name == "SetUp")
            {
                attributes.Add(new ButtonAttribute("Create Core File"));
                attributes.Add(new PropertyOrderAttribute(100));
            }

            if (member.Name == "RefreshSaveInfoList")
            {
                attributes.Add(new ButtonAttribute());
                attributes.Add(new PropertyOrderAttribute(110));
            }
            
            if (member.Name == "PlaySave")
            {
                attributes.Add(new ButtonAttribute(ButtonSizes.Large));
                attributes.Add(new PropertyOrderAttribute(120));
            }
            
            if (member.Name == "ResetSave")
            {
                attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                attributes.Add(new PropertyOrderAttribute(140));
            }
        }
    }
}
