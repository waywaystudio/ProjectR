using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public float TestSecond;
    public bool BooleanToggle;
    
    [Button]
    public void Debugger()
    {
        var waitTrigger = new WaitTrigger(Log, () => BooleanToggle);
        
        waitTrigger.Pull();
    }
    
    [Button]
    public void TimeDebugger()
    {
        var timeTrigger = new TimeTrigger(Log, TestSecond);
        
        timeTrigger.Pull();
    }

    public void Log() => Debug.Log(DateTime.Now);
}
