using System;
using System.Collections.Generic;
using System.Reflection;
using Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable UnusedType.Global

namespace Editor
{
    public class SaveInfoDrawer : OdinAttributeProcessor<SaveInfo>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideReferenceObjectPickerAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "filename")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new DisplayAsStringAttribute());
                attributes.Add(new HorizontalGroupAttribute("Main", 0.8f));
                attributes.Add(new VerticalGroupAttribute("Main/Left"));
            }
            
            if (member.Name == "saveTime")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new DisplayAsStringAttribute());
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Left"));
            }
            
            if (member.Name == "filePath")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new DisplayAsStringAttribute());
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Left"));
            }

            if (member.Name == "Save")
            {
                attributes.Add(new ButtonAttribute(SdfIconType.Save));
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Right"));
            }
            
            if (member.Name == "Load")
            {
                attributes.Add(new ButtonAttribute(SdfIconType.Download));
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Right"));
            }

            if (member.Name == "Delete")
            {
                attributes.Add(new ButtonAttribute(SdfIconType.Recycle));
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new VerticalGroupAttribute("Main/Right"));
            }
        }
    }

    public class SaveManagerDrawer : OdinAttributeProcessor<SaveManager>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "SaveFileDirectory")
            {
                attributes.Add(new TitleGroupAttribute("Config", "const, static", order:-1f));
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "PlaySaveName")
            {
                attributes.Add(new TitleGroupAttribute("Config"));
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "Extension")
            {
                attributes.Add(new TitleGroupAttribute("Config"));
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "SaveFileDirectory")
            {
                attributes.Add(new TitleGroupAttribute("Config", "const, static"));
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "saveInfoList")
            {
                attributes.Add(new TitleGroupAttribute("Current SaveFiles", "Data/SaveFile"));
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    Expanded = true,
                    IsReadOnly = true,
                });
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
            }
            
            if (member.Name == "listenerList")
            {
                attributes.Add(new ShowInPlayModeAttribute());
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    Expanded   = true,
                    IsReadOnly = true,
                });
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
            }
            
            if (member.Name == "LoadAllSaveFile")
            {
                attributes.Add(new ButtonGroupAttribute("EditorFunctions"));
                attributes.Add(new ButtonAttribute
                {
                    Icon          = SdfIconType.Download,
                    ButtonHeight  = 32,
                    IconAlignment = IconAlignment.LeftEdge,
                });
            }
            
            if (member.Name == "OpenSaveFilePath")
            {
                attributes.Add(new ButtonGroupAttribute("EditorFunctions"));
                attributes.Add(new ButtonAttribute
                {
                    Icon          = SdfIconType.DoorOpen,
                    ButtonHeight  = 32,
                    IconAlignment = IconAlignment.LeftEdge,
                });
                attributes.Add(new PropertyOrderAttribute(10));
            }

            if (member.Name == "ResetFile")
            {
                attributes.Add(new ButtonGroupAttribute("EditorFunctions"));
                attributes.Add(new ButtonAttribute
                {
                    Icon          = SdfIconType.X,
                    ButtonHeight  = 32,
                    IconAlignment = IconAlignment.LeftEdge,
                });
                attributes.Add(new PropertyOrderAttribute(99));
            }

            if (member.Name == "CreateNewSaveFile")
            {
                attributes.Add(new TitleGroupAttribute("Creation Menu", "just New or with current", order:-0.5f));
                attributes.Add(new ButtonAttribute
                {
                    Expanded = true,
                });
            }
            
            if (member.Name == "SaveToNewFile")
            {
                attributes.Add(new TitleGroupAttribute("Creation Menu"));
                attributes.Add(new ButtonAttribute
                {
                    Expanded = true,
                });
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
            }
        }
    }

    public class SaveEventListenerDrawer : OdinAttributeProcessor<SaveListener>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Save")
            {
                attributes.Add(new ButtonAttribute
                {
                    Icon         = SdfIconType.Upload,
                    Name         = "Manual Save",
                    ButtonHeight = 24,
                });
                attributes.Add(new HorizontalGroupAttribute("SerializeEvents"));
                attributes.Add(new VerticalGroupAttribute("SerializeEvents/Left"));
                attributes.Add(new PropertyOrderAttribute(-2f));
            }
            
            if (member.Name == "saveEvent")
            {
                attributes.Add(new HorizontalGroupAttribute("SerializeEvents"));
                attributes.Add(new VerticalGroupAttribute("SerializeEvents/Left"));
                attributes.Add(new PropertyOrderAttribute(-1f));
            }

            if (member.Name == "Load")
            {
                attributes.Add(new ButtonAttribute
                {
                    Icon         = SdfIconType.Download,
                    Name         = "Manual Load",
                    ButtonHeight = 24,
                });
                attributes.Add(new HorizontalGroupAttribute("SerializeEvents"));
                attributes.Add(new VerticalGroupAttribute("SerializeEvents/Right"));
                attributes.Add(new PropertyOrderAttribute(-2f));
            }

            if (member.Name == "loadEvent")
            {
                attributes.Add(new HorizontalGroupAttribute("SerializeEvents"));
                attributes.Add(new VerticalGroupAttribute("SerializeEvents/Right"));
                attributes.Add(new PropertyOrderAttribute(-1f));
            }
        }
    }
}
