using Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public bool Toggle;
    public LayerMask layer;
    
    [Button]
    private void Debugger()
    {
        // Debug.Log($"byNameToLayer {layer == LayerMask.NameToLayer("Adventurer")}");
        // Debug.Log($"byGetMask {layer == LayerMask.GetMask("Adventurer")}");
        // Debug.Log($"byNameToLayer {layer.value == LayerMask.NameToLayer("Adventurer")}");
        // Debug.Log($"byGetMask {layer.value == LayerMask.GetMask("Adventurer")}");
        // Debug.Log($"layerToName just layer {LayerMask.LayerToName(layer)}");
        // Debug.Log($"layerToName layer.value {LayerMask.LayerToName(layer.value)}");
        // Debug.Log($"just layer, layer.Value {layer.value == layer}");
        
        
        // Debug.Log($"just Layer : {layer}");
        // Debug.Log($"Layer.Value : {layer.value}");
        // Debug.Log($"LayerMask.LayerToName : {LayerMask.LayerToName(gameObject.layer)}");
        Debug.Log($"LayerMask.NameToLayer(Adventurer) : {LayerMask.NameToLayer("Adventurer")}");

        var objectLayer = gameObject.layer;
        
        Debug.Log($"just objectLayer : {objectLayer}");
        Debug.Log($"just objectLayer : {objectLayer}");
        // Debug.Log($"objectLayer LayerMask.LayerToName : {LayerMask.LayerToName(objectLayer)}");
        // Debug.Log($"objectLayer GetMask : {LayerMask.GetMask(LayerMask.LayerToName(objectLayer))}");
    }

}
