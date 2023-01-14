using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RightAction1 : Action
{
    public override void OnAwake()
    {
        // combat = GetComponent<CombatBehaviour>();
    }
    
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
