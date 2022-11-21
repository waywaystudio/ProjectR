using System;
using Common.Character;
using Main;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody characterRigidbody;
    [SerializeField] private CharacterAnimationModel animationModel;

    private Vector3 direction;

    private void Awake()
    {
        characterRigidbody ??= GetComponent<Rigidbody>();
        animationModel ??= GetComponentInChildren<CharacterAnimationModel>();
    }

    public void Initialize(Rigidbody rigidBody)
    {
        characterRigidbody = rigidBody;
    }

    // UnityEvent :: PlayerInput.Events.Player.Move - WASD move
    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        
        direction = new Vector3(input.x, 0f, input.y);
        
        if (input.x == 0.0f && input.y == 0.0f) StateTo(CharacterState.Idle);
        if (input.x != 0.0f || input.y != 0.0f) StateTo(CharacterState.Run);
        
        animationModel.Flip(direction);

        if (context.canceled)
        {
            direction = Vector3.zero;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        StateTo(CharacterState.Attack);
    }

    // UnityEvent :: PlayerInput.Events.Player.Interaction - 'E' Interaction
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        MainGame.InputManager.InvokeEvents();
    }

    public void DirectionControl()
    {
        characterRigidbody.velocity = direction * (moveSpeed * Time.deltaTime);
    }

    [Button]
    public void DuplicateTest() => StateTo(CharacterState.Attack | CharacterState.Idle);

    public void StateTo(CharacterState state)
    {
        if (state.HasFlag(CharacterState.Idle))
        {
            animationModel.Idle();
        }
        
        if (state.HasFlag(CharacterState.Attack))
        {
            animationModel.Attack();
        }
        
        if (state.HasFlag(CharacterState.Run))
        {
            animationModel.Run();
        }
        
        if (state.HasFlag(CharacterState.Crouch))
        {
            animationModel.Crouch();
        }
    }

    private void OnDisable()
    {
        if (MainGame.Instance is null) return;

        MainGame.InputManager.Unregister();
    }
}