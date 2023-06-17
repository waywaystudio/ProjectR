using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class ActionTableDrawer : OdinAttributeProcessor<ActionTable> 
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Table")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new DictionaryDrawerSettings
                {
                    ValueLabel = "Actions",
                });
            }
        }
    }
    
    public class ActionTableDrawer<T> : OdinAttributeProcessor<ActionTable<T>> 
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Table")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new DictionaryDrawerSettings
                {
                    ValueLabel = "Actions",
                });
            }
        }
    }
    
    public class ConditionTableDrawer : OdinAttributeProcessor<ConditionTable> 
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "table")
            {
                attributes.Add(new ShowInPlayModeAndInspectorAttribute());
            }
        }
    }
}
