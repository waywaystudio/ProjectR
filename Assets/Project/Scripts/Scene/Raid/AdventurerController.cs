using Character;
using Core;
using MainGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Raid
{
    public class AdventurerController : MonoBehaviour
    {
        // TODO. TEMP
        [SerializeField] private AdventurerBehaviour focusedAdventurer;

        private Camera mainCamera;
        private Vector3 mouseDestination;
        
        public AdventurerBehaviour FocusedAdventurer => focusedAdventurer;

        public void OnFocusChanged(AdventurerBehaviour ab) => focusedAdventurer = ab;
        
        public void Move(InputAction.CallbackContext context)
        {
            if (!focusedAdventurer.isActiveAndEnabled) return;
            if (!MainManager.Input.TryGetMousePosition(out var mouse)) return;
            if (MainManager.Input.IsMouseOnUI) return;

            focusedAdventurer.ActionBehaviour.Run(mouse);
        }

        public void Teleport(InputAction.CallbackContext context)
        {
            if (!focusedAdventurer.isActiveAndEnabled) return;
            
            focusedAdventurer.ActionBehaviour.Teleport();
        }

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            if (!MainManager.Input.TryGetAction(BindingCode.LeftMouse, out var moveAction)) return;
            
            moveAction.started += Move;

            if (!MainManager.Input.TryGetAction(BindingCode.Space, out var teleportAction)) return;

            teleportAction.started += Teleport;
        }

        private void OnDisable()
        {
            if (MainManager.Input is null) return;
            
            if (!MainManager.Input.TryGetAction(BindingCode.LeftMouse, out var moveAction)) return;

            moveAction.started -= Move;

            if (!MainManager.Input.TryGetAction(BindingCode.Space, out var teleportAction)) return;

            teleportAction.started -= Teleport;
        }
    }
}
