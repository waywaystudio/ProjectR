using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    private Vector3 newPosition;
    private Camera mainCamera;
    
    private void Start () 
    {
        mainCamera = Camera.main;
        newPosition = transform.position;
    }
    
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (!Physics.Raycast(ray, out var hit)) return;
        
        newPosition = hit.point;
        transform.position = newPosition;
    }
}
