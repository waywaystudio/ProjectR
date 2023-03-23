using Adventurers;
using Character.Adventurers;
using GameEvents;
using Manager;
using Raid.Art;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Raid
{
    public class AdventurerController : MonoBehaviour
    {
        // TODO. TEMP
        [SerializeField] private Adventurer focusedAdventurer;
        [SerializeField] private ToMoveProjector moveProjector;
        //

        [SerializeField] private GameEventAdventurer onFocusChanged;
        [SerializeField] private GameEvent onCommandMode; 

        private Vector3 mouseDestination;


        public void OnFocusChanged(Adventurer ab) => focusedAdventurer = ab;

        /* GameEvent */
        public void CommandMode(InputAction.CallbackContext context)
        {
            // onFocusChanged.Invoke(null);
            onCommandMode.Invoke();
        }
        
        public void Move(InputAction.CallbackContext context)
        {
            if (focusedAdventurer.IsNullOrEmpty()) return;
            if (!focusedAdventurer.isActiveAndEnabled) return;
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;
            if (MainManager.Input.IsMouseOnUI) return;

            moveProjector.Projecting();
            focusedAdventurer.Run(mousePosition);
        }

        public void Teleport(InputAction.CallbackContext context)
        {
            if (focusedAdventurer.IsNullOrEmpty()) return;
            if (!focusedAdventurer.isActiveAndEnabled) return;
            
            // focusedAdventurer.ActionBehaviour.Teleport();
        }

        private void ToFirstAdventurer(InputAction.CallbackContext context)
        {
            if (RaidDirector.AdventurerList.Count > 0)
                RaidDirector.ChangeFocusAdventurer(RaidDirector.AdventurerList[0]);
        }
        
        private void ToSecondAdventurer(InputAction.CallbackContext context)
        {
            if (RaidDirector.AdventurerList.Count > 1) 
                RaidDirector.ChangeFocusAdventurer(RaidDirector.AdventurerList[1]);
        }
        
        private void ToThirdAdventurer(InputAction.CallbackContext context)
        {
            if (RaidDirector.AdventurerList.Count > 2)
                RaidDirector.ChangeFocusAdventurer(RaidDirector.AdventurerList[2]);
        }


        private void Register()
        {
            if (MainManager.Input.TryGetAction(BindingCode.LeftMouse, out var moveAction))
                moveAction.started += Move;

            if (MainManager.Input.TryGetAction(BindingCode.Space, out var teleportAction))
                teleportAction.started += Teleport;
            
            if (MainManager.Input.TryGetAction(BindingCode.G, out var commandAction))
                commandAction.started += CommandMode;

            if (MainManager.Input.TryGetAction(BindingCode.Keyboard1, out var focusFirst))
                focusFirst.started += ToFirstAdventurer;
            
            if (MainManager.Input.TryGetAction(BindingCode.Keyboard2, out var focusSecond))
                focusSecond.started += ToSecondAdventurer;
            
            if (MainManager.Input.TryGetAction(BindingCode.Keyboard3, out var focusThird))
                focusThird.started += ToThirdAdventurer;
        }

        private void Unregister()
        {
            if (MainManager.Input is null) return;
            
            if (MainManager.Input.TryGetAction(BindingCode.LeftMouse, out var moveAction))
                moveAction.started -= Move;

            if (MainManager.Input.TryGetAction(BindingCode.Space, out var teleportAction))
                teleportAction.started -= Teleport;
            
            if (MainManager.Input.TryGetAction(BindingCode.G, out var commandAction))
                commandAction.started -= CommandMode;
            
            if (MainManager.Input.TryGetAction(BindingCode.Keyboard1, out var focusFirst))
                focusFirst.started -= ToFirstAdventurer;
            
            if (MainManager.Input.TryGetAction(BindingCode.Keyboard2, out var focusSecond))
                focusSecond.started -= ToSecondAdventurer;
            
            if (MainManager.Input.TryGetAction(BindingCode.Keyboard3, out var focusThird))
                focusThird.started -= ToThirdAdventurer;
        }

        private void Start() => Register();
        private void OnDisable() => Unregister();
    }
}
