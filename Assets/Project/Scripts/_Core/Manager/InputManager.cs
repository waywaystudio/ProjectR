using Inputs;
using Singleton;
using UnityEngine;
using UnityEngine.InputSystem;
using Context = UnityEngine.InputSystem.InputAction.CallbackContext;

public class InputManager : MonoSingleton<InputManager>
{
    [SerializeField] private InputActionAsset playerInputAsset;
    [SerializeField] private PlayerInputMap raid;

    private InputActionMap raidMap;

    public static PlayerInputMap Raid => Instance.raid;

    private static InputActionAsset InputAsset => Instance.playerInputAsset;
    private static InputActionMap RaidMap => Instance.raidMap;


    // UI Over control
    public static void OnRaidMap() => RaidMap.Enable();
    public static void OffRaidMap() => RaidMap.Disable();
    
    public static Vector3 GetMousePosition(float groundHeight = 0f)
    {
        var mousePosition = Vector3.negativeInfinity;
        var plane = new Plane(Vector3.up, groundHeight);
        var ray = Camera.main!.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out var distance))
            mousePosition = ray.GetPoint(distance);

        return mousePosition;
    }
    

    protected override void Awake()
    {
        base.Awake();

        raidMap = InputAsset.FindActionMap("Raid");
        raid.Initialize(playerInputAsset);
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetSingleton()
    {
        if (!Instance.IsNullOrDestroyed())
            Instance.SetInstanceNull();
    }
}
