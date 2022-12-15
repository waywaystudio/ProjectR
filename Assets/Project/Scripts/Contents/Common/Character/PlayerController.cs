using System;
using Common.Character;
using Common.Character.Graphic;
using MainGame;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody characterRigidbody;
    [SerializeField] private AnimationModel animationModel;

    private Vector3 direction;

    private void Awake()
    {
        characterRigidbody ??= GetComponent<Rigidbody>();
        animationModel ??= GetComponentInChildren<AnimationModel>();
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
        // animationModel.Flip(direction);

        if (context.canceled)
        {
            direction = Vector3.zero;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }

    // UnityEvent :: PlayerInput.Events.Player.Interaction - 'E' Interaction
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        MainManager.InputManager.InvokeEvents();
    }

    public void DirectionControl()
    {
        characterRigidbody.velocity = direction * (moveSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        if (MainManager.Instance is null) return;

        MainManager.InputManager.Unregister();
    }
}