using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public Action CsharpAction;
    
    [OdinSerialize]
    public Action OdinAction;

    [OdinSerialize]
    public Dictionary<int, int> TableTest;

    public SandSphere sandSphere;


    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        
    }
    public void A() { Debug.Log("A");}
    public void B() { Debug.Log("B");}
    public void C() { Debug.Log("C");}
    public void D() { Debug.Log("D");}
}
