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
    [SerializeField] private Vector3 billboardAngle = new (45, 0, 0);

    private void Awake()
    {
        SetAngle();
    }

    private void Update()
    {
        if (!transform.hasChanged) return;
        
        SetAngle();
    }

    private void SetAngle() => transform.eulerAngles = billboardAngle;
}

#if UNITY_EDITOR && ODIN_INSPECTOR
public class BillboardProtocolDrawer : OdinAttributeProcessor<BillboardProtocol>
{
    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        if (member.Name == "SetAngle")
        {
            attributes.Add(new ButtonAttribute(ButtonSizes.Large));
        }
    }
}
#endif
