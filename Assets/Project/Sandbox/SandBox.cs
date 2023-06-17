using System;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public float TestSecond;
    public bool BooleanToggle;
    
    [Button]
    public void CoolDebugger()
    {
        // var coolTImer = new CoolTimer(TestSecond, Log);
        
        // coolTImer.Play();
    }
    
    [Button]
    public void CastDebugger()
    {
        // var castTImer = new CastTimer(TestSecond, Log);
        
        // castTImer.Play();
    }

    public void Log() => Debug.Log(DateTime.Now);
}
