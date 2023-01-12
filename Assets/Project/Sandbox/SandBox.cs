using Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public LayerMask layer;
    
    [Button]
    private void Debugger()
    {
        Debug.Log($"just Layer : {layer}");
        Debug.Log($"Layer.Value : {layer.value}");
        Debug.Log($"LayerMask.LayerToName : {LayerMask.LayerToName(layer.value)}");
        Debug.Log($"LayerMask.NameToLayer(Adventurer) : {LayerMask.NameToLayer("Adventurer")}");

        var objectLayer = gameObject.layer;
        
        Debug.Log($"just objectLayer : {objectLayer}");
        Debug.Log($"objectLayer LayerMask.LayerToName : {LayerMask.LayerToName(objectLayer)}");
    }

}
