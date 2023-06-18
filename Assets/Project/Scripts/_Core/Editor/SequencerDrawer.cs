using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor
{
    public class SequencerDrawer : OdinAttributeProcessor<Sequencer>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
            attributes.Add(new HideReferenceObjectPickerAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Condition")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new PropertyOrderAttribute(10f));
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new FoldoutGroupAttribute("Condition", true));
            }

            if (member.Name == "ActiveAction")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(12f));
                attributes.Add(new FoldoutGroupAttribute("Active"));
            }
            
            if (member.Name == "Table")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new HideReferenceObjectPickerAttribute());
                attributes.Add(new PropertyOrderAttribute(13f));
                // attributes.Add(new FoldoutGroupAttribute("Cancel"));
            }
            
            if (member.Name == "CancelAction")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(13f));
                attributes.Add(new FoldoutGroupAttribute("Cancel"));
            }
            
            if (member.Name == "CompleteAction")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(14f));
                attributes.Add(new FoldoutGroupAttribute("Complete"));
            }
            
            if (member.Name == "EndAction")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(15f));
                attributes.Add(new FoldoutGroupAttribute("End"));
            }
        }
    }
    
    public class SequencerDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : Sequencer<T1>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new ShowInInspectorAttribute());
            attributes.Add(new HideLabelAttribute());
            attributes.Add(new HideReferenceObjectPickerAttribute());
        }
        
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Condition")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new PropertyOrderAttribute(10f));
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new FoldoutGroupAttribute("Condition", true));
            }
            
            if (member.Name == "ActiveParamAction")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(11f));
                attributes.Add(new FoldoutGroupAttribute("ActiveParameter"));
            }
            
            if (member.Name == "Table")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new HideReferenceObjectPickerAttribute());
                attributes.Add(new PropertyOrderAttribute(13f));
                // attributes.Add(new FoldoutGroupAttribute("Cancel"));
            }
            
            if (member.Name == "ActiveAction")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(12f));
                attributes.Add(new FoldoutGroupAttribute("Active"));
            }
            
            if (member.Name == "CancelAction")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(13f));
                attributes.Add(new FoldoutGroupAttribute("Cancel"));
            }
            
            if (member.Name == "CompleteAction")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(14f));
                attributes.Add(new FoldoutGroupAttribute("Complete"));
            }
            
            if (member.Name == "EndAction")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new PropertyOrderAttribute(15f));
                attributes.Add(new FoldoutGroupAttribute("End"));
            }
        }
    }
}
