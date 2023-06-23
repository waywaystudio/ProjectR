using System;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public float StandardValue;
    public float Haste;
    // public bool BooleanToggle;
    
    [Button]
    public void CoolDebugger()
    {
        var hasteValue = CombatFormula.GetHasteValue(Haste);
        Debug.Log($"Decrease : StandardValue * Haste = {StandardValue * hasteValue}");
        Debug.Log($"Increase : StandardValue * 1 + Haste = {StandardValue * (1.0f + hasteValue)}");
    }
    
    [Button]
    public void CastDebugger()
    {
        // var castTImer = new CastTimer(TestSecond, Log);
        
        // castTImer.Play();
    }

    public void Log() => Debug.Log(DateTime.Now);
}
