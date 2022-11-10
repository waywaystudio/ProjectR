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

        // TODO. Scene에서 초기 IControlModel을 잡아주어야 한다.
        Register();
        MainGame.CameraManager.SetFollow(transform);
    }

    public void Register() => MainGame.InputManager.Register(this);
    public void UpdateState()
    {
        if (MainGame.InputManager.GetPermission(this) is false)
            return;
        
        DirectionControl();
    }

    public void Unregister()
    {
        MainGame.InputManager.Unregister();
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

        MainGame.InputManager.InvokeEvent();
    }

    private void DirectionControl()
    {
        // TODO. 스킵하는 조건을 만들고 싶은데, Vector3.Zero 했더니 안 멈춘다;
        // if (direction.Equals(Vector3.zero)) return;
        
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
        
        Unregister();
        MainGame.CameraManager.ReleaseFollow();
    }
}
