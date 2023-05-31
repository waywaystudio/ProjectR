using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        // Debug.Log($"{relicType.CovertToVirtue().ToString()}");
    }
}
