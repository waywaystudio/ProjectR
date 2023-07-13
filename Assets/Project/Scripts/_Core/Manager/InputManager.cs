using Inputs;
using Singleton;
using UnityEngine;
using UnityEngine.InputSystem;
using Context = UnityEngine.InputSystem.InputAction.CallbackContext;

public class InputManager : MonoSingleton<InputManager>
{
    [SerializeField] private InputActionAsset playerInput;
    [SerializeField] private PlayerInputMap raid;

    public static PlayerInputMap Raid => Instance.raid;


    protected override void Awake()
    {
        base.Awake();

        raid.Initialize(playerInput);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetSingleton()
    {
        if (!Instance.IsNullOrDestroyed())
            Instance.SetInstanceNull();
    }
}
