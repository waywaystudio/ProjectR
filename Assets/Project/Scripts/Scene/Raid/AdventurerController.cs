using Character;
using Core;
using MainGame;
using UnityEngine;
using UnityEngine.EventSystems;
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
        
        public void Move(InputAction.CallbackContext context)
        {
            if (!focusedAdventurer.isActiveAndEnabled) return;
            if (!MainManager.Input.TryGetMousePosition(out var mouse)) return;
            
            // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.2/manual/UISupport.html#ui-and-game-input
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("It's UI Object");
                return;
            }
            
            focusedAdventurer.ActionBehaviour.Run(mouse);
        }

        public void Dash(InputAction.CallbackContext context)
        {
            if (!focusedAdventurer.isActiveAndEnabled) return;
            
            focusedAdventurer.ActionBehaviour.Dash();
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
            if (!MainManager.Input.TryGet(BindingCode.LeftMouse, out var moveAction)) return;

            moveAction.started += Move;
            
            if (!MainManager.Input.TryGet(BindingCode.LeftShift, out var dashAction)) return;

            dashAction.started += Dash;
            
            if (!MainManager.Input.TryGet(BindingCode.Space, out var teleportAction)) return;

            teleportAction.started += Teleport;
        }

        private void OnDisable()
        {
            if (MainManager.Input is null) return;
            
            if (!MainManager.Input.TryGet(BindingCode.LeftMouse, out var moveAction)) return;

            moveAction.started -= Move;
            
            if (!MainManager.Input.TryGet(BindingCode.LeftShift, out var dashAction)) return;

            dashAction.started -= Dash;
            
            if (!MainManager.Input.TryGet(BindingCode.Space, out var teleportAction)) return;

            teleportAction.started -= Teleport;
        }
    }
}
