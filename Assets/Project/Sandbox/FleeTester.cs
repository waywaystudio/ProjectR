using System.Linq;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

public class FleeTester : MonoBehaviour
{
    public Vector3 Destination;
    
    [SerializeField] private Seeker agent;
    [SerializeField] private int searchLength = 500;
    [SerializeField, PropertyRange(0, 1)] private float strength = 1f;
    [SerializeField] private int spread = 500;
    [SerializeField] private Transform destinationObject;
    
    private FleePath path;
    private Camera mainCamera;
    private Vector3 dropPosition;

    private void Awake()
    {
        agent ??= GetComponent<Seeker>();
        mainCamera = Camera.main;
        Destination = Vector3.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        SetFleePath();
    }

    private void SetFleePath()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit)) return;
        
        dropPosition = hit.point;
        //
        Debug.Log("Flee In");
        path = FleePath.Construct(transform.position, dropPosition, searchLength);
        path.aimStrength = strength;
        path.spread = spread;
        agent.StartPath(path, OnComplete);
    }

    private void OnComplete(Path p)
    {
        Destination = p.vectorPath.Last();
        destinationObject.position = Destination;
    }
}
