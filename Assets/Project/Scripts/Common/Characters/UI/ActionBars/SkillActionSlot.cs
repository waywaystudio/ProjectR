using Manager;
using TMPro;
using UnityEngine;

namespace Common.Characters.UI.ActionBars
{
    public class SkillActionSlot : MonoBehaviour, IEditable
    {
        [SerializeField] private BindingCode bindingCode;
        [SerializeField] private TextMeshProUGUI hotKey;
        [SerializeField] private SkillSymbol symbol;

        private string HotKey =>
            bindingCode switch
            {
                BindingCode.Q => "Q",
                BindingCode.W => "W",
                BindingCode.E => "E",
                BindingCode.R => "R",
                _             => "-",
            };


        public void UpdateSlot(DataIndex dataIndex)
        {
            symbol.UpdateSymbol(dataIndex);
        }
        

        private void Awake()
        {
            hotKey.text = HotKey;
        }

        private void OnEnable()
        {
            if (!MainManager.Input.TryGetAction(bindingCode, out var inputAction))
            {
                Debug.LogWarning($"Not exist InputAction by {bindingCode}");
                return;
            }

            if (symbol.IsNullOrEmpty())
            {
                Debug.LogWarning("Not exist Symbol.");
                return;
            }

            inputAction.started  += symbol.StartAction;
            inputAction.canceled += symbol.ReleaseAction;
        }

        private void OnDisable()
        {
            if (MainManager.Input.IsNullOrEmpty()) return;
            if (!MainManager.Input.TryGetAction(bindingCode, out var inputAction))
            {
                Debug.LogWarning($"Not exist InputAction by {bindingCode}");
                return;
            }

            inputAction.started  -= symbol.StartAction;
            inputAction.canceled -= symbol.ReleaseAction;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            hotKey = transform.Find("Hotkey")
                              .GetComponent<TextMeshProUGUI>();
            hotKey.text = HotKey;
            
            symbol = transform.Find("SymbolHierarchy")
                              .Find("Symbol")
                              .GetComponent<SkillSymbol>();
            
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void EditorPersonalSetUp(BindingCode bindingCode, DataIndex skillCode)
        {
            this.bindingCode = bindingCode;

            EditorSetUp();

            if (symbol.IsNullOrEmpty()) return;
            
            symbol.EditorPersonalSetUp(skillCode);
        }
#endif
    }
}
