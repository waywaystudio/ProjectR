using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Common.UI.ActionBars
{
    public class ActionSlot : MonoBehaviour, IEditable
    {
        [SerializeField] protected BindingCode hotKey;
        [SerializeField] protected TextMeshProUGUI hotKeyTextUI;
        // [SerializeField] protected ActionSymbol actionSymbol;
        [SerializeField] protected UnityEvent<InputAction.CallbackContext> OnActivated;
        [SerializeField] protected UnityEvent<InputAction.CallbackContext> OnReleased;


        // public void RegisterSymbol(IAssignable symbol) => actionSymbol.SetSymbol(symbol);
        // public void UnregisterSymbol() => actionSymbol.SetEmpty();


        protected void OnEnable()
        {
            // if (!MainManager.Input.TryGetAction(hotKey, out var inputAction)) return;
            //
            // inputAction.started  += OnActivated.Invoke;
            // inputAction.canceled += OnReleased.Invoke;
        }
        
        protected void OnDisable()
        {
            // if (!MainManager.Input.TryGetAction(hotKey, out var inputAction)) return;
            //
            // inputAction.started  -= OnActivated.Invoke;
            // inputAction.canceled -= OnReleased.Invoke;
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            hotKeyTextUI          ??= GetComponentInChildren<TextMeshProUGUI>();
            // actionSymbol          ??= GetComponentInChildren<ActionSymbol>();
            
            hotKeyTextUI.text = hotKey.ToString() == "None" 
                ? string.Empty 
                : hotKey.ToString();
        }
#endif
    }
}
