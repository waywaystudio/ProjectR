using System;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public RelicType relicType;
    public int endDigit;

    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        Debug.Log($"{relicType.CovertToVirtue().ToString()}");
    }
}
