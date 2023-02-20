using System;
using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MainGame.Manager.Input
{
    public class InputManager : MonoBehaviour
    {
        private bool isMouseOnUI;
        private ProjectInput projectInput;
        private Camera mainCamera;

        public bool IsMouseOnUI => isMouseOnUI;

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
                _ => null,
            };

            if (inputAction is null)
                Debug.LogError($"Not Assignable Keycode. Input:{bindingCode.ToString()}. "
                               + $"Check InputManager.Get(BindingCode keyCode) function.");

            return inputAction != null;
        }

        public bool TryGetMousePosition(out Vector3 mousePosition)
        {
            mousePosition = Vector3.negativeInfinity;
                
            var plane = new Plane(Vector3.up, 0f);
            var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            // Check Zero Plane
            if (!plane.Raycast(ray, out var distance)) return false;

            mousePosition = ray.GetPoint(distance);
            return true;
        }


        private void Awake()
        {
            projectInput = new ProjectInput();
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            foreach (var value in Enum.GetValues(typeof(BindingCode)))
            {
                var bindingCode = (BindingCode)value;
                        
                if (bindingCode == BindingCode.None) continue;
                if (!TryGetAction(bindingCode, out var inputAction)) continue;
                        
                // InputTable.Add(bindingCode, new InputActionTable());
                        
                inputAction.Enable();
                // inputAction.started  += InputTable[bindingCode].OnStarted.Invoke;
                // inputAction.canceled += InputTable[bindingCode].OnCanceled.Invoke;
            }
        }

        
        /// TODO. 황당한 케이스; InputSystem에서 UI 클릭 or not 구분하기 위한 방법;
        private void Update()
        {
            // if (!Mouse.current.leftButton.wasPressedThisFrame) return;

            isMouseOnUI = EventSystem.current.IsPointerOverGameObject(PointerInputModule.kMouseLeftId);
        }

        private void OnDisable()
        {
            foreach (var value in Enum.GetValues(typeof(BindingCode)))
            {
                var bindingCode = (BindingCode)value;
                        
                if (bindingCode == BindingCode.None) continue;
                if (!TryGetAction(bindingCode, out var inputAction)) continue;
                        
                // InputTable.Add(bindingCode, new InputActionTable());
                        
                inputAction.Enable();
                // inputAction.started  += InputTable[bindingCode].OnStarted.Invoke;
                // inputAction.canceled += InputTable[bindingCode].OnCanceled.Invoke;
            }
        }
        
    }
}

// private Dictionary<BindingCode, InputActionTable> InputTable { get; } = new();

// private void OnEnable()
// {
//     foreach (var value in Enum.GetValues(typeof(BindingCode)))
//     {
//         var bindingCode = (BindingCode)value;
//                 
//         if (bindingCode == BindingCode.None) continue;
//         if (!TryGet(bindingCode, out var inputAction)) continue;
//                 
//         InputTable.Add(bindingCode, new InputActionTable());
//                 
//         inputAction.Enable();
//         inputAction.started  += InputTable[bindingCode].OnStarted.Invoke;
//         inputAction.canceled += InputTable[bindingCode].OnCanceled.Invoke;
//     }
// }
//
// private void OnDisable()
// {
//     foreach (var value in Enum.GetValues(typeof(BindingCode)))
//     {
//         var bindingCode = (BindingCode)value;
//                 
//         if (bindingCode == BindingCode.None) continue;
//         if (!TryGet(bindingCode, out var inputAction)) continue;
//
//                 
//         inputAction.started  -= InputTable[bindingCode].OnStarted.Invoke;
//         inputAction.canceled -= InputTable[bindingCode].OnCanceled.Invoke;
//         inputAction.Disable();
//     }
//             
//     InputTable.Clear();
// }

// public void Register(BindingCode bindingCode, string actionKey, Action startedAction, Action canceledAction)
// {
//     if (!TryGet(bindingCode, out _)) return;
//     if (!InputTable.ContainsKey(bindingCode)) 
//         InputTable.Add(bindingCode, new InputActionTable());
//             
//     InputTable[bindingCode].Register(actionKey, startedAction, canceledAction);
// }
//
// public void Unregister(BindingCode bindingCode, string actionKey)
// {
//     if (!TryGet(bindingCode, out _)) return;
//     if (!InputTable.ContainsKey(bindingCode)) return;
//             
//     InputTable[bindingCode].Unregister(actionKey);
// }

// private class InputActionTable
// {
//     public ActionTable<InputAction.CallbackContext> OnStarted { get; } = new();
//     public ActionTable<InputAction.CallbackContext> OnCanceled { get; } = new();
//
//     public void Register(string key, Action startedAction, Action canceledAction)
//     {
//         OnStarted.Register(key, startedAction);
//         OnCanceled.Register(key, canceledAction);
//     }
//
//     public void Unregister(string key)
//     {
//         OnStarted.Unregister(key);
//         OnCanceled.Unregister(key);
//     }
// }
