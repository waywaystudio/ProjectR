#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

namespace Core
{
    public interface ISavable
    {        
        void Save();
        void Load();
    }
    
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
}
