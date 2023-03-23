using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Manager.Input
{
    public class InputManager : MonoBehaviour
    {
        private bool isMouseOnUI;
        private ProjectInput projectInput;
        private readonly RaycastHit[] buffers = new RaycastHit[8];

        public bool IsMouseOnUI => isMouseOnUI;
        public Camera MainCamera { get; set; }

        public bool TryGetAction(BindingCode bindingCode, out InputAction inputAction)
        {
            inputAction = bindingCode switch
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
                BindingCode.G => projectInput.Raid.G,
                _ => null,
            };

            if (inputAction is null)
                Debug.LogError($"Not Assignable Keycode. Input:{bindingCode.ToString()}. "
                               + "Check InputManager.TryGetAction(BindingCode bindingCode) function.");

            return inputAction != null;
        }

        public bool TryGetMousePosition(out Vector3 mousePosition)
        {
            mousePosition = Vector3.negativeInfinity;
                
            var plane = new Plane(Vector3.up, 0f);
            var ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            // Check Zero Plane
            if (!plane.Raycast(ray, out var distance)) return false;

            mousePosition = ray.GetPoint(distance);
            return true;
        }
        
        public bool TryGetGroundPosition(out Vector3 groundPosition)
        {
            groundPosition = Vector3.negativeInfinity;
            var ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.RaycastNonAlloc(ray, buffers, 1000f, LayerMask.GetMask("Ground")) == 0)
            {
                groundPosition = Vector3.negativeInfinity;
                return false;
            }

            foreach (var hit in buffers)
            {
                if (hit.collider.IsNullOrEmpty()) break;
                
                groundPosition = hit.point;
                break;
            }

            Array.Clear(buffers, 0, buffers.Length);
            return groundPosition != Vector3.negativeInfinity;
        }

        public void InputOn()
        {
            foreach (var value in Enum.GetValues(typeof(BindingCode)))
            {
                var bindingCode = (BindingCode)value;
                        
                if (bindingCode == BindingCode.None) continue;
                if (!TryGetAction(bindingCode, out var inputAction)) continue;

                inputAction.Enable();
            }
        }

        /* TODO.씬 로딩사이에 잠시 인풋을 꺼주고 싶은데, 다시 켜주는 시점 설정을 못잡았다.
         Enabled, Start, Awake는 안된다. InputManager GameObject는 꺼지지 않는다.*/
        public void InputOff()
        {
            foreach (var value in Enum.GetValues(typeof(BindingCode)))
            {
                var bindingCode = (BindingCode)value;
                        
                if (bindingCode == BindingCode.None) continue;
                if (!TryGetAction(bindingCode, out var inputAction)) continue;

                inputAction.Disable();
            }
        }


        private void Awake()
        {
            projectInput = new ProjectInput();
            MainCamera ??= Camera.main;
        }

        private void OnEnable() => InputOn();
        // private void OnDisable() => InputOff();

        /// TODO. 황당한 케이스; InputSystem에서 UI 클릭 or not 구분하기 위한 방법;
        private void Update()
        {
            isMouseOnUI = EventSystem.current.IsPointerOverGameObject(PointerInputModule.kMouseLeftId);
        }
    }
}
