using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class TableDrawer<TKey, TValue> : OdinAttributeProcessor<Table<TKey, TValue>>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "keyList")
            {
                attributes.Add(new HideIfAttribute("hideKey"));
                attributes.Add(new DisplayAsStringAttribute());
                attributes.Add(new HorizontalGroupAttribute("Main", 0.3f));
                attributes.Add(new ListDrawerSettingsAttribute()
                {
                    ShowFoldout = true,
                    HideAddButton = true,
                    HideRemoveButton = true,
                    IsReadOnly = true,
                    NumberOfItemsPerPage = 20,
                });
            }
            
            if (member.Name == "valueList")
            {
                attributes.Add(new HorizontalGroupAttribute("Main"));
                attributes.Add(new ListDrawerSettingsAttribute()
                {
                    ShowFoldout          = true,
                    HideAddButton        = true,
                    HideRemoveButton     = true,
                    IsReadOnly           = true,
                    NumberOfItemsPerPage = 20,
                });
            }
        }

    }
}
