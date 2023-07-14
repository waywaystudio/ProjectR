using Character.Venturers;
using GameEvents;
using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Raid
{
    using Context = InputAction.CallbackContext;
    
    public class RaidInputDirector : InputDirector
    {
        // TODO. 여기가 아닌 것 같다.
        [SerializeField] private GameEvent onCommandMode;
        //
        
        private static bool ValidVenturer => !FocusVenturer.IsNullOrEmpty() || !FocusVenturer.isActiveAndEnabled;
        private static VenturerBehaviour FocusVenturer
        {
            get => RaidDirector.FocusVenturer;
            set => RaidDirector.FocusVenturer = value;
        }

        public override void Initialize()
        {
            base.Initialize();

            this["G"].AddStart("EnterCommandMode", EnterCommandMode);
            
            this["LeftMouse"].AddStart("VenturerMove", Move);
            this["Keyboard1"].AddStart("Focusing", () => Focusing(0));
            this["Keyboard2"].AddStart("Focusing", () => Focusing(1));
            this["Keyboard3"].AddStart("Focusing", () => Focusing(2));
            this["Keyboard4"].AddStart("Focusing", () => Focusing(3));
            this["Keyboard5"].AddStart("Focusing", () => Focusing(4));
            this["Keyboard6"].AddStart("Focusing", () => Focusing(5));
        }


        private void EnterCommandMode()
        {
            onCommandMode.Invoke();
        }
        
        private void Move(Context context)
        {
            if (IsOnUI) return;
            if (!ValidVenturer) return;
            
            var groundPosition = InputManager.GetMousePosition();
            
            FocusVenturer.Run(groundPosition);
        }

        private static void Focusing(int index)
        {
            if (!ValidVenturerList(index)) return;

            FocusVenturer = RaidDirector.VenturerList[index];
        }

        private static bool ValidVenturerList(int index)
        {
            return RaidDirector.VenturerList.Count > index && 
                   RaidDirector.VenturerList[index] != null;
        }
    }
}
