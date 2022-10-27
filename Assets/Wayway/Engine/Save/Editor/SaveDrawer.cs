using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Wayway.Engine.Save.Core;
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace Wayway.Engine.Save.Editor
{
    public class SaveDrawer
    {
        
    }
    
    public class SaveEventListenerDrawer : OdinAttributeProcessor<SaveEventListener>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "targetEventList")
            {
                attributes.Add(new HideInInspector());
            }
            
            if (member.Name == "priority")
            {
                attributes.Add(new HideInInspector());
            }
        }
    }
}
