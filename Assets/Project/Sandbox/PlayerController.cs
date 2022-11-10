using Main;
using Main.Input;
using Main.Save;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IControlModel, ISavable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody rigidbody3D;

    private Vector3 direction;

    private void Awake()
    {
        rigidbody3D ??= GetComponent<Rigidbody>();
        
        MainGame.InputManager.Register(this);
    }

    public void UpdateState()
    {
        if (MainGame.InputManager.GetPermission(this) is false)
            return;
        
        DirectionControl();
    }

    // UnityEvent :: PlayerInput.Events.Player.Move - WASD move
    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0f, input.y);

        if (context.canceled)
        {
            direction = Vector3.zero;
        }
    }

    // UnityEvent :: PlayerInput.Events.Player.Interaction - 'E' Interaction
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        MainGame.InputManager.InvokeEvents();
    }

    private void DirectionControl()
    {
        rigidbody3D.velocity = direction * (moveSpeed * Time.deltaTime);
    }

    public void Save()
    {
        MainGame.SaveManager.Save("playerTransform.position", transform.position);
        MainGame.SaveManager.Save("playerTransform.rotation", transform.rotation);
    }

    public void Load()
    {
        var position = MainGame.SaveManager.Load("playerTransform.position", Vector3.zero);
        var rotation = MainGame.SaveManager.Load("playerTransform.rotation", Quaternion.identity);
        
        transform.SetPositionAndRotation(position, rotation);
    }

    private void OnDisable()
    {
        if (MainGame.Instance is null) return;

        MainGame.InputManager.Unregister();
    }
}
