using System;
using Cinemachine;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public CinemachineImpulseSource Cis;
    public float StandardValue;
    public float Haste;
    // public bool BooleanToggle;
    
    [Button]
    public void Impulse()
    {
        var randomVelocity = new Vector3(
            UnityEngine.Random.Range(-1f, 1f), 
            UnityEngine.Random.Range(-1f, 1f), 
            UnityEngine.Random.Range(-1f, 1f)
        );
        
        Cis.GenerateImpulseWithVelocity(randomVelocity);
    }
    
    [Button]
    public void CastDebugger()
    {
        // var castTImer = new CastTimer(TestSecond, Log);
        
        // castTImer.Play();
    }

    public void Log() => Debug.Log(DateTime.Now);
}
