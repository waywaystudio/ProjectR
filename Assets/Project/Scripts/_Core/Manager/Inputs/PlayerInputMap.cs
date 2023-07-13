using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    using Context = InputAction.CallbackContext;
    
    public class PlayerInputMap : MonoBehaviour
    {
        [SerializeField] private string mapId;
        
        private Dictionary<string, InputActionTable> StartActionTable { get; } = new();
        private Dictionary<string, InputActionTable> PerformedActionTable { get; } = new();

        public InputActionTable this[string binding, InputSection section]
        {
            get
            {
                var table = section switch
                {
                    InputSection.Start   => StartActionTable,
                    InputSection.Perform => PerformedActionTable,
                    _                    => throw new ArgumentOutOfRangeException(nameof(section), section, null)
                };

                return table is null 
                    ? null 
                    : table[binding];
            }
        }

        public void Initialize(InputActionAsset playerInput)
        {
            var inputMap = playerInput.FindActionMap(mapId);

            inputMap?.actions.ForEach(inputAction =>
            {
                var inputKey = inputAction.name;

                StartActionTable.TryAdd(inputKey, new InputActionTable());
                PerformedActionTable.TryAdd(inputKey, new InputActionTable());
                
                inputAction.started   += StartActionTable[inputKey].Invoke;
                inputAction.performed += PerformedActionTable[inputKey].Invoke;
            });
        }
    }
}
