using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainGame.Manager.Input
{
    public class InputManager : MonoBehaviour
    {
        private ProjectInput projectInput;


        public bool TryGet(BindingCode keyCode, out InputAction inputAction)
        {
            inputAction = keyCode switch
            {
                BindingCode.Keyboard1 => projectInput.Raid.Keyboard1,
                BindingCode.Keyboard2 => projectInput.Raid.Keyboard2,
                BindingCode.Keyboard3 => projectInput.Raid.Keyboard3,
                BindingCode.Keyboard4 => projectInput.Raid.Keyboard4,
                BindingCode.LeftMouse => projectInput.Raid.LeftMouse,
                BindingCode.RightMouse => projectInput.Raid.RightMouse,
                BindingCode.LeftShift => projectInput.Raid.LeftShift,
                BindingCode.Space => projectInput.Raid.Space,
                BindingCode.Q => projectInput.Raid.Q,
                BindingCode.W => projectInput.Raid.W,
                BindingCode.E => projectInput.Raid.E,
                BindingCode.R => projectInput.Raid.R,
                BindingCode.A => projectInput.Raid.A,
                BindingCode.S => projectInput.Raid.S,
                BindingCode.D => projectInput.Raid.D,
                BindingCode.F => projectInput.Raid.F,
                _ => null,
            };

            if (inputAction is null)
                Debug.LogError($"Not Assignable Keycode. Input:{keyCode.ToString()}. "
                               + $"Check InputManager.Get(BindingCode keyCode) function.");

            return inputAction != null;
        }

        private void Awake()
        {
            projectInput = new ProjectInput();
        }
    }
}
