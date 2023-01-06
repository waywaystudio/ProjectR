using Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public LayerMask layer;
    
    [Button]
    private void Debugger()
    {
        var sandboxLayer = gameObject.layer;
        
        Debug.Log(sandboxLayer);
    }

}
