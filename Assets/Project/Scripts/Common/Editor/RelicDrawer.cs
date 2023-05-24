using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class RelicTableDrawer : OdinAttributeProcessor<RelicTable>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
            attributes.Add(new HideReferenceObjectPickerAttribute());
            attributes.Add(new HideLabelAttribute());
        }
        
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "CurrentRelicType")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "RelicEthos")
            {
                attributes.Add(new HideIfAttribute("CurrentRelicType", RelicType.None));
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "EnchantSpec")
            {
                attributes.Add(new HideIfAttribute("CurrentRelicType", RelicType.None));
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "NextRelic")
            {
                attributes.Add(new ButtonAttribute());
            }
        }
    }
    
    
}
