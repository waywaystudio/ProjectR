using System;
using System.Collections.Generic;
using System.Reflection;
using Databases;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class DataObjectDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : DataObject<T1> where T1 : class, IIdentifier
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "list")
            {
                attributes.Add(new SearchableAttribute());
                attributes.Add(new TableListAttribute
                {
                    AlwaysExpanded = true,
                    HideToolbar = true,
                    DrawScrollView = true,
                    IsReadOnly = true,
                });
            }

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
    
    public class ResourceDataDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : ResourceData<T1> where T1 : UnityEngine.Object
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "searchingTargetPath")
            {
                attributes.Add(new FolderPathAttribute());
            }
            
            if (member.Name == "resourceList")
            {
                attributes.Add(new PropertySpaceAttribute(15f, 0f));
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    IsReadOnly = true,
                    Expanded = true,
                });
            }
        }
    }
    
    public class ResourceTableDrawer<T> : OdinAttributeProcessor<ResourceData<T>.ResourceTable> where T : UnityEngine.Object
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "DataIndex")
            {
                attributes.Add(new VerticalGroupAttribute("Table/Property"));
                attributes.Add(new PropertyOrderAttribute(1f));
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "Category")
            {
                attributes.Add(new VerticalGroupAttribute("Table/Property"));
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new PropertyOrderAttribute(2f));
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "Resource")
            {
                attributes.Add(new HorizontalGroupAttribute("Table", 0.12f));
                attributes.Add(new PropertyOrderAttribute(0f));
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PreviewFieldAttribute
                {
                    Height = 40f,
                    Alignment = ObjectFieldAlignment.Left
                });
            }
        }
    }
}
