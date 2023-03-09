using System;
using System.Collections.Generic;
using System.Reflection;
using Databases;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace MainEditor
{
    public class DataObjectDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : DataObject<T1> where T1 : class, IIdentifier
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
            // attributes.Add(new InlineEditorAttribute(InlineEditorObjectFieldModes.Foldout))
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "list")
            {
                attributes.Add(new TableListAttribute
                {
                    AlwaysExpanded = true,
                    HideToolbar = true,
                    DrawScrollView = true,
                    IsReadOnly = true
                });
            }
            
            // if (member.Name == "LoadFromJson")
            // {
            //     attributes.Add(new PropertySpaceAttribute(5f, 0f));
            //     attributes.Add(new ButtonAttribute(ButtonSizes.Medium)
            //     {
            //         Icon = SdfIconType.ArrowRepeat
            //     });
            // }
            
            if (member.Name == "LoadFromGoogleSpreadSheet")
            {
                attributes.Add(new PropertyOrderAttribute(-1f));
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                    Icon = SdfIconType.ArrowRepeat,
                    Stretch = false,
                });
            }
        }
    }
}
