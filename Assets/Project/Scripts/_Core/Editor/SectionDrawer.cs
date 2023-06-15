using System;
using System.Collections.Generic;
using System.Reflection;
using Sequences;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class OldSectionDrawer : OdinAttributeProcessor<OldSection>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideReferenceObjectPickerAttribute());
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "AwaitEvent")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class SectionDrawer : OdinAttributeProcessor<Section>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideReferenceObjectPickerAttribute());
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "sectionType")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(-1));
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "ActionTable")
            {
                attributes.Add(new ShowInPlayModeAndInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
            }
        }
    }
    
    public class SectionParameterDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : OldSection<T1>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideReferenceObjectPickerAttribute());
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "AwaitEvent")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
    
    public class SequencerDrawer : OdinAttributeProcessor<Sequencer>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
            attributes.Add(new HideReferenceObjectPickerAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "conditionTable")
            {
                attributes.Add(new PropertyOrderAttribute(10f));
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new FoldoutGroupAttribute("Condition", true));
            }
            
            if (member.Name == "activeSection")
            {
                attributes.Add(new PropertyOrderAttribute(12f));
                attributes.Add(new FoldoutGroupAttribute("Active"));
            }
            
            if (member.Name == "cancelSection")
            {
                attributes.Add(new PropertyOrderAttribute(13f));
                attributes.Add(new FoldoutGroupAttribute("Cancel"));
            }
            
            if (member.Name == "completeSection")
            {
                attributes.Add(new PropertyOrderAttribute(14f));
                attributes.Add(new FoldoutGroupAttribute("Complete"));
            }
            
            if (member.Name == "endSection")
            {
                attributes.Add(new PropertyOrderAttribute(15f));
                attributes.Add(new FoldoutGroupAttribute("End"));
            }
        }
    }
    
    public class SequencerDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : Sequencer<T1>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
            attributes.Add(new HideReferenceObjectPickerAttribute());
        }
        
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "key")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "conditionTable")
            {
                attributes.Add(new PropertyOrderAttribute(10f));
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new FoldoutGroupAttribute("Condition", true));
            }
            
            if (member.Name == "activeParamSection")
            {
                attributes.Add(new PropertyOrderAttribute(11f));
                attributes.Add(new FoldoutGroupAttribute("ActiveParameter"));
            }
            
            if (member.Name == "activeSection")
            {
                attributes.Add(new PropertyOrderAttribute(12f));
                attributes.Add(new FoldoutGroupAttribute("Active"));
            }
            
            if (member.Name == "cancelSection")
            {
                attributes.Add(new PropertyOrderAttribute(13f));
                attributes.Add(new FoldoutGroupAttribute("Cancel"));
            }
            
            if (member.Name == "completeSection")
            {
                attributes.Add(new PropertyOrderAttribute(14f));
                attributes.Add(new FoldoutGroupAttribute("Complete"));
            }
            
            if (member.Name == "endSection")
            {
                attributes.Add(new PropertyOrderAttribute(15f));
                attributes.Add(new FoldoutGroupAttribute("End"));
            }
        }
    }
}
