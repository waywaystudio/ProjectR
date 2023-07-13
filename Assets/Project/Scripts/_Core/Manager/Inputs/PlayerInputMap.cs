using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    using Context = InputAction.CallbackContext;
    
    public class PlayerInputMap : MonoBehaviour
    {
        [SerializeField] private PlayerInputMapType mapType;

        private Dictionary<string, ActionTable<Context>> StartActionTable { get; } = new();
        private Dictionary<string, ActionTable<Context>> PerformedActionTable { get; } = new();

        public ActionTable<Context> this[InputSection section, string binding]
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
            var inputMap = mapType switch
            {
                PlayerInputMapType.Raid => playerInput.FindActionMap("Raid"),
                PlayerInputMapType.UI   => playerInput.FindActionMap("UI"),
                _                       => null,
            };

            if (inputMap is null) return;

            foreach (var inputAction in inputMap.actions)
            {
                var inputKey = inputAction.name;
                
                StartActionTable.TryAdd(inputKey, new ActionTable<Context>());
                PerformedActionTable.TryAdd(inputKey, new ActionTable<Context>());
                
                inputAction.started   += StartActionTable[inputKey].Invoke;
                inputAction.performed += PerformedActionTable[inputKey].Invoke;
            }
        }
    }
}
