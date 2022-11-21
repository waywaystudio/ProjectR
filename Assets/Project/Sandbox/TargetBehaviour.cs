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
}