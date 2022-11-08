using Main;
using Main.Manager.Control;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IControllable
{
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private Rigidbody rigidbody3D;
    
    private Vector3 direction;

    private void Awake()
    {
        rigidbody3D ??= GetComponent<Rigidbody>();

        // TODO. Scene에서 초기 IControllable을 잡아주어야 한다.
        Register();
    }

    public void Reaction()
    {
        DirectionControl();
        // InteractionControl();
    }

    public void Register() => MainGame.ControlManager.Register(this);

    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        
        direction = new Vector3(input.x, 0f, input.y);
    }

    private void DirectionControl()
    {
        // TODO. 스킵하는 조건을 만들고 싶은데, Vector3.Zero 했더니 안 멈춘다;
        // if (direction.Equals(Vector3.zero)) return;
        
        rigidbody3D.velocity = direction * (moveSpeed * Time.deltaTime);
    }

    private void InteractionControl()
    {
        
    }
}
