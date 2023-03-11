#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector.Editor;
#endif
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;


namespace Manager.Save
{
    public interface ISavable
    {        
        void Save();
        void Load();
    }

#if UNITY_EDITOR
    public class SavableDrawer : OdinAttributeProcessor<ISavable>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Save")
            {
                attributes.Add(new PropertyOrderAttribute(9999));
                attributes.Add(new HorizontalGroupAttribute("ISavable"));
                attributes.Add(new ButtonAttribute("Manual Save"));
            }
            
            if (member.Name == "Load")
            {
                attributes.Add(new HorizontalGroupAttribute("ISavable"));
                attributes.Add(new ButtonAttribute("Manual Load"));
            }
        }
    }
#endif
}
