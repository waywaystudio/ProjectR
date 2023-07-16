using Character.Venturers;
using Inputs;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Lobby
{
    using Context = InputAction.CallbackContext;
    
    public class LobbyInputDirector : InputDirector
    {
        private static bool ValidVenturer => !FocusVenturer.IsNullOrEmpty() || !FocusVenturer.isActiveAndEnabled;
        private static VenturerBehaviour FocusVenturer
        {
            get => LobbyDirector.FocusVenturer;
            set => LobbyDirector.FocusVenturer = value;
        }

        public UnityEvent<InputAction.CallbackContext> InteractAction { get; set; }
        public override void Initialize()
        {
            base.Initialize();
            
            this["LeftMouse"].AddStart("VenturerMove", Move);
            this["A"].AddStart("EnterCommandMode", Interact);
        }


        private void Move(Context context)
        {
            if (IsMouseOnUI) return;
            if (!ValidVenturer) return;
            
            var groundPosition = InputManager.GetMousePosition();
            
            FocusVenturer.Run(groundPosition);
        }

        private void Interact(InputAction.CallbackContext context)
        {
            if (InteractAction == null) return;
            if (!ValidVenturer) return;
            
            InteractAction?.Invoke(context);
            FocusVenturer.Stop();
        }
    }
}
