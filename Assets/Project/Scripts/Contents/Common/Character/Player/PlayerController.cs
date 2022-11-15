using Core;
using Main;
using Main.Manager.Save;
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
    }

    private void Start()
    {
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
        // 씬에 따라서 저장해야 하는 정보가 다름 특히 위치
        // 세이브가 가능한 Town씬에서는 위치를 저장하는게 의미가 있음.
        // SaveManager.Save("playerTransform.position", transform.position);
        // SaveManager.Save("playerTransform.rotation", transform.rotation);
    }

    public void Load()
    {
        // 마찬가지로 씬에 따라서 불러와야 하는 정보가 다를 수 있음.
        // 레이드 씬에서는 플레이어 위치정보를 불러올 필요가 없음.
        // var position = SaveManager.Load("playerTransform.position", Vector3.zero);
        // var rotation = SaveManager.Load("playerTransform.rotation", Quaternion.identity);
        // 
        // transform.SetPositionAndRotation(position, rotation);
    }

    private void OnDisable()
    {
        if (MainGame.Instance is null) return;

        MainGame.InputManager.Unregister();
    }
}
