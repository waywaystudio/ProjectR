using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    /*
     * 작동 안함.
     */
    public class ActionTableDrawer : OdinAttributeProcessor<ActionTable> 
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            // attributes.Add(new ShowInInspectorAttribute());
            // attributes.Add(new HideReferenceObjectPickerAttribute());
        }
        
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            
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
