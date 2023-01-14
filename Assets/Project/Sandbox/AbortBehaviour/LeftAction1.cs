using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class LeftAction1 : Action
{
    private SandBox sandBox;
    
    public override void OnAwake()
    {
        sandBox = GetComponent<SandBox>();
    }
    
    public override TaskStatus OnUpdate()
    {
        return sandBox.Toggle
                ? TaskStatus.Success
                : TaskStatus.Failure;
    }
}
