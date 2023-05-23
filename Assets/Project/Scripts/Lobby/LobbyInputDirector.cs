using System;
using Character.Adventurers;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Lobby
{
    public class LobbyInputDirector : MonoBehaviour
    {
        private Adventurer focusedAdventurer;

        public UnityEvent<InputAction.CallbackContext> InteractAction { get; set; }

        public void Initialize(Adventurer adventurer) => focusedAdventurer = adventurer;
        
        public void Move(InputAction.CallbackContext context)
        {
            if (focusedAdventurer.IsNullOrEmpty()) return;
            if (!focusedAdventurer.isActiveAndEnabled) return;
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;
            if (MainManager.Input.IsMouseOnUI) return;

            focusedAdventurer.Run(mousePosition);
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (InteractAction == null) return;
            if (focusedAdventurer.IsNullOrEmpty()) return;
            if (!focusedAdventurer.isActiveAndEnabled) return;
            
            InteractAction?.Invoke(context);
            focusedAdventurer.Stop();
        }
        
        private void Register()
        {
            if (MainManager.Input.TryGetAction(BindingCode.LeftMouse, out var moveAction))
                moveAction.started += Move;
            
            if (MainManager.Input.TryGetAction(BindingCode.A, out var interactAction))
                interactAction.started += Interact;
        }

        private void Unregister()
        {
            if (MainManager.Input is null) return;
            
            if (MainManager.Input.TryGetAction(BindingCode.LeftMouse, out var moveAction))
                moveAction.started -= Move;
            
            if (MainManager.Input.TryGetAction(BindingCode.A, out var interactAction))
                interactAction.started -= Interact;
        }

        private void Start() => Register();
        private void OnDisable() => Unregister();
    }
}
