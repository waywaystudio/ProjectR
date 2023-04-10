using System;
using Common;
using Manager;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public CombatClassType SampleClassType;
    public int NextIteratorCount;
    // public Action CsharpAction;
    // public SandSphere sandSphere;

    // [SerializeField] private Processor processor = new();

    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        var typeBuffer = SampleClassType;
        
        for (var i = 0; i < NextIteratorCount; i++)
        {
            typeBuffer = typeBuffer.NextClass();
        }
        
        Debug.Log(typeBuffer);
    }

    // [Button]
    // public void SetProgress(float value) => processor.Value = value;
    //
    // [Button]
    // public void SetEndTime(float value) => processor.EndTime = (value);
    //
    // [Button]
    // public void GetProgress()
    // {
    //     Debug.Log(processor.Value);
    // }
    
    public void A() { Debug.Log("A");}
    public void B() { Debug.Log("B");}
    public void C() { Debug.Log("C");}
    public void D() { Debug.Log("D");}
}
