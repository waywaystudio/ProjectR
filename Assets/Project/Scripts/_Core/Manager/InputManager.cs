using Inputs;
using Singleton;
using UnityEngine;
using UnityEngine.InputSystem;
using Context = UnityEngine.InputSystem.InputAction.CallbackContext;

public class InputManager : MonoSingleton<InputManager>
{
    private static Camera MainCamera => CameraManager.MainCamera;
    private static InputDirector Director { get; set; }

    [Sirenix.OdinInspector.ShowInInspector]
    public string MapId => Director is null ? "Not yet" : Director.name;


    public static void SetDirector(InputDirector director) => Director = director;

    public static Vector3 GetMousePosition(float groundHeight = 0f)
    {
        var mousePosition = Vector3.negativeInfinity;
        var plane = new Plane(Vector3.up, groundHeight);

        var ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out var distance))
        {
            mousePosition = ray.GetPoint(distance);
        }
        else
        {
            Debug.LogWarning($"Not Exist {groundHeight} Level Plane");
        }

        return mousePosition;
    }
    
    public static bool TryGetMousePosition(out Vector3 mousePosition, float groundHeight = 0f)
    {
        mousePosition = GetMousePosition(groundHeight);
    
        return mousePosition != Vector3.negativeInfinity;
    }


#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetSingleton()
    {
        if (!Instance.IsNullOrDestroyed())
            Instance.SetInstanceNull();
    }
#endif
}
