using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class EthosEntityDrawer : OdinAttributeProcessor<EthosEntity>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
            attributes.Add(new HideReferenceObjectPickerAttribute());
            attributes.Add(new HideLabelAttribute());
        }
        
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "ethosType")
            {
                attributes.Add(new HorizontalGroupAttribute("StatValue", 0.25f));
                attributes.Add(new HideLabelAttribute());
            }
            
            if (member.Name == "ethosKey")
            {
                attributes.Add(new DisplayAsStringAttribute());
                attributes.Add(new HorizontalGroupAttribute("StatValue", 0.3f));
                attributes.Add(new HideLabelAttribute());
            }
            
            if (member.Name == "value")
            {
                attributes.Add(new HorizontalGroupAttribute("StatValue"));
                attributes.Add(new HideLabelAttribute());
            }
        }
    }
    
    public class EthosSpecDrawer : OdinAttributeProcessor<EthosSpec>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
            attributes.Add(new HideReferenceObjectPickerAttribute());
            attributes.Add(new HideLabelAttribute());
        }
    
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "ethosList")
            {
                attributes.Add(new LabelTextAttribute("Spec"));
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    IsReadOnly = true,
                    Expanded   = true,
                });
            }
        }
    }
    
    public class EthosTableDrawer : OdinAttributeProcessor<EthosTable>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
            attributes.Add(new HideReferenceObjectPickerAttribute());
            attributes.Add(new HideLabelAttribute());
        }
        
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Table")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class EthosSetDrawer : OdinAttributeProcessor<EthosTable.EthosSet>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "table")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "Value")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
