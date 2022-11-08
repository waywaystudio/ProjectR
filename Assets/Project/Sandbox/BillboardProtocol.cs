using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

public class BillboardProtocol : MonoBehaviour
{
    private void Awake()
    {
        if (Math.Abs(transform.eulerAngles.x - 45.0f) > 0.0001f)
        {
            SetAngle();
        }
    }
    
    private void SetAngle() => transform.eulerAngles = new Vector3(45, 0, 0);
}

#if UNITY_EDITOR && ODIN_INSPECTOR
public class BillboardProtocolDrawer : OdinAttributeProcessor<BillboardProtocol>
{
    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        if (member.Name == "SetAngle")
        {
            attributes.Add(new ButtonAttribute());
        }
    }
}
#endif
