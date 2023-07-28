using Character.Venturers;
using Inputs;
using UnityEngine.InputSystem;

namespace Raid
{
    using Context = InputAction.CallbackContext;
    
    public class RaidInputDirector : InputDirector
    {
        private static bool ValidVenturer => !FocusVenturer.IsNullOrEmpty() && FocusVenturer.isActiveAndEnabled;
        private static VenturerBehaviour FocusVenturer
        {
            get => RaidDirector.FocusVenturer;
            set => RaidDirector.FocusVenturer = value;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            this["LeftMouse"].AddStart("VenturerMove", MoveVenturer);

            this["G"].AddStart("EnterCommandMode", RaidDirector.CommandMode);
            this["Keyboard1"].AddStart("Focusing", () => Focusing(0));
            this["Keyboard2"].AddStart("Focusing", () => Focusing(1));
            this["Keyboard3"].AddStart("Focusing", () => Focusing(2));
            this["Keyboard4"].AddStart("Focusing", () => Focusing(3));
            this["Keyboard5"].AddStart("Focusing", () => Focusing(4));
            this["Keyboard6"].AddStart("Focusing", () => Focusing(5));
        }

        public void OnCommandModeEnter()
        {
            DisablePlayerControl();
        }

        public void OnCommandModeExit()
        {
            this["LeftMouse"].AddStart("VenturerMove", MoveVenturer);
            
            this["Keyboard1"].AddStart("Focusing", () => Focusing(0));
            this["Keyboard2"].AddStart("Focusing", () => Focusing(1));
            this["Keyboard3"].AddStart("Focusing", () => Focusing(2));
            this["Keyboard4"].AddStart("Focusing", () => Focusing(3));
            this["Keyboard5"].AddStart("Focusing", () => Focusing(4));
            this["Keyboard6"].AddStart("Focusing", () => Focusing(5));
        }

        public void OnRaidWin()
        {
            DisablePlayerControl();
        }
        
        public void OnRaidDefeat()
        {
            DisablePlayerControl();
        }
        

        private void MoveVenturer()
        {
            if (IsMouseOnUI) return;
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

        private void DisablePlayerControl()
        {
            this["LeftMouse"].RemoveStart("VenturerMove");
            
            this["Keyboard1"].RemoveStart("Focusing");
            this["Keyboard2"].RemoveStart("Focusing");
            this["Keyboard3"].RemoveStart("Focusing");
            this["Keyboard4"].RemoveStart("Focusing");
            this["Keyboard5"].RemoveStart("Focusing");
            this["Keyboard6"].RemoveStart("Focusing");
        }
    }
}
