using System;
using Core;
using Main;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IControlModel, ISavable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody rigidbody3D;
    [SerializeField] private HeroAnimationModel animationModel;

    private Vector3 direction;

    private void Awake()
    {
        rigidbody3D ??= GetComponent<Rigidbody>();
        animationModel ??= GetComponentInChildren<HeroAnimationModel>();
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
        
        if (input.x == 0.0f && input.y == 0.0f) StateTo(PlayerState.Idle);
        if (input.x != 0.0f || input.y != 0.0f) StateTo(PlayerState.Run);
        
        // animationModel.SetDirection(input);

        if (context.canceled)
        {
            direction = Vector3.zero;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        StateTo(PlayerState.Attack);
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

    [Button]
    public void DuplicateTest() => StateTo(PlayerState.Attack | PlayerState.Idle);

    public void StateTo(PlayerState state)
    {
        // if (state.HasFlag(PlayerState.Idle))
        // {
        //     Debug.Log("Idle");
        //     animationModel.Idle();
        // }
        //
        // if (state.HasFlag(PlayerState.Attack))
        // {
        //     Debug.Log("Attack");
        //     animationModel.Attack();
        // }
        //
        // if (state.HasFlag(PlayerState.Run))
        // {
        //     Debug.Log("Run");
        //     animationModel.Run();
        // }
        //
        // if (state.HasFlag(PlayerState.Crouch))
        // {
        //     Debug.Log("Crouch");
        //     animationModel.Crouch();
        // }
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

[Flags]
public enum PlayerState
{
    Idle = 1 << 0,
    Attack = 1 << 1,
    Run = 1 << 2,
    Crouch = 1 << 3,
    All = int.MaxValue
}

