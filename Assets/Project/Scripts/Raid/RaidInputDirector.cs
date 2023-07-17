using Character.Venturers;
using Inputs;
using UnityEngine;
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

            this["G"].AddStart("EnterCommandMode", RaidDirector.CommandMode);

            this["Keyboard1"].AddStart("Focusing", () => Focusing(0));
            this["Keyboard2"].AddStart("Focusing", () => Focusing(1));
            this["Keyboard3"].AddStart("Focusing", () => Focusing(2));
            this["Keyboard4"].AddStart("Focusing", () => Focusing(3));
            this["Keyboard5"].AddStart("Focusing", () => Focusing(4));
            this["Keyboard6"].AddStart("Focusing", () => Focusing(5));
        }

        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            if (vb == null) return;
            
            // TODO. Focusing 변겨할 때 마다 매번 들어오게 하지 않는 방법 없을까?
            this["LeftMouse"].AddStart("VenturerMove", MoveVenturer);
        }

        // TODO. Command Mode Input을 넣어야 하는데, 이곳에서 넣는게 아닌 듯 하다.
        // 혹은, 다른 곳에서 내용을 만들고, 이곳에서 접근하여 넣을 수도 있다.
        // UI Q,W,E,R 등록하는 방식과 맥락을 맞추자.
        // 여튼 Command Mode에 대한 기획이 끝나면 해보자.
        public void OnCommandMode()
        {
            this["LeftMouse"].RemoveStart("VenturerMove");
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
    }
}
