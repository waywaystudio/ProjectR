using System;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public Stat StatA = new (StatType.CriticalChance, "SandBox1", 13);
    public Stat StatB = new (StatType.CriticalChance, "SandBox2", 7); 
    public Stat StatC;
    
    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        
    }
}
