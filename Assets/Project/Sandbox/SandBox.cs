using System;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public StatEntity StatA = new (StatType.CriticalChance, "SandBox1", 13);
    public StatEntity StatB = new (StatType.CriticalChance, "SandBox2", 7); 
    public StatEntity StatC;
    
    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        
    }
}
