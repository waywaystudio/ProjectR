using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Editor
{
    public class PoolDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : Pool<T1> where T1 : MonoBehaviour
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            
            attributes.Add(new HideLabelAttribute());
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "prefab")
            {
                attributes.Add(new TitleGroupAttribute("Pooling Property", "prefab, count and sync"));
            }
            
            if (member.Name == "maxCount")
            {
                attributes.Add(new TitleGroupAttribute("Pooling Property"));
                attributes.Add(new PropertyRangeAttribute(1, 64));
            }
            
            if (member.Name == "syncObjectActivity")
            {
                attributes.Add(new TitleGroupAttribute("Pooling Property", "prefab, count and sync"));
                attributes.Add(new PropertySpaceAttribute(0f, 15f));
                attributes.Add(new TooltipAttribute("풀링되는 게임오브젝트의 Activity를 시퀀스에 따라서 true(=OnGet), false(=OnRelease)하며 "
                                                    + "Pool을 Clear할 때 해당 게임오브젝트를 삭제한다."));
            }
        }
    }
}
