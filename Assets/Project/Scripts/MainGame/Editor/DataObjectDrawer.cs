using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace MainGame.Editor
{
    public class DataObjectDrawer : OdinAttributeProcessor
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "category")
            {
                attributes.Add(new HideInInspector());
            }
            
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
            
            if (member.Name == "LoadFromJson")
            {
                attributes.Add(new PropertySpaceAttribute(5f, 0f));
                attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
            }
            
            if (member.Name == "LoadFromGoogleSpreadSheet")
            {
                attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
            }
        }
    }
}
