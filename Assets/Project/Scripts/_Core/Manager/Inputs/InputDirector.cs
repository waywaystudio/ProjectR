using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inputs
{
    using Context = InputAction.CallbackContext;
    
    public class InputDirector : MonoBehaviour
    {
        [SerializeField] private InputActionAsset playerInputAsset;
        [SerializeField] private string mapId;

        private readonly PointerEventData pointerEventData = new(EventSystem.current);
        private readonly List<RaycastResult> raycastResultsList = new();
        private bool initialized;

        protected bool IsOnUI
        {
            get
            {
                pointerEventData.position = Mouse.current.position.ReadValue();
                raycastResultsList.Clear();

                EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

                foreach (var result in raycastResultsList)
                {
                    if (result.gameObject.TryGetComponent(out Graphic _))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private Dictionary<string, InputActionTable> InputActionTable { get; } = new();

        public InputActionTable this[string binding] => InputActionTable[binding];


        public virtual void Initialize()
        {
            InputManager.SetDirector(this);
            
            var inputActionMap = playerInputAsset.FindActionMap(mapId);
            
            inputActionMap?.actions.ForEach(inputAction =>
            {
                var inputKey = inputAction.name;

                InputActionTable.TryAdd(inputKey, new InputActionTable());

                inputAction.started   += InputActionTable[inputKey].StartInvoke;
                inputAction.canceled  += InputActionTable[inputKey].CancelInvoke;
                inputAction.performed += InputActionTable[inputKey].PerformInvoke;
            });

            initialized = true;
        }


        protected void Clear()
        {
            if (!initialized) return;
            
            var inputActionMap = playerInputAsset.FindActionMap(mapId);
            
            inputActionMap?.actions.ForEach(inputAction =>
            {
                var inputKey = inputAction.name;
                
                inputAction.started   -= InputActionTable[inputKey].StartInvoke;
                inputAction.canceled  -= InputActionTable[inputKey].CancelInvoke;
                inputAction.performed -= InputActionTable[inputKey].PerformInvoke;
            });
            
            InputActionTable.Clear();
        }
        
        private void OnDestroy()
        {
            Clear();
        }
    }
}
