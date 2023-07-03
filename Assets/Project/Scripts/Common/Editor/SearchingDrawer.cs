using System;
using System.Collections.Generic;
using System.Reflection;
using Common.TargetSystem;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class SearchingDrawer : OdinAttributeProcessor<SearchEngine>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "SearchedTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
