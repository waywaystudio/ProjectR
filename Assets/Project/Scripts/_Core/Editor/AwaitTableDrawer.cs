using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class AwaitTableDrawer : OdinAttributeProcessor<AwaitTable>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "actionTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    ShowFoldout = true,
                    IsReadOnly  = true,
                });
            }
            
            if (member.Name == "awaitTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    ShowFoldout = true,
                    IsReadOnly = true,
                });
            }
        }
    }
    
    public class AwaitTableDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : AwaitTable<T1>
    {

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "actionTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    ShowFoldout = true,
                    IsReadOnly  = true,
                });
            }
            
            if (member.Name == "awaitTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    ShowFoldout = true,
                    IsReadOnly  = true,
                });
            }
        }
    }
}
