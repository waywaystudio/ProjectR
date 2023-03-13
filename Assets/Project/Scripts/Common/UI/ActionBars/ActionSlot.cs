using TMPro;
using UnityEngine;

namespace Common.UI.ActionBars
{
    public class ActionSlot : MonoBehaviour, IEditable
    {
        [SerializeField] protected BindingCode hotKey;
        [SerializeField] protected Transform skillObjectHierarchy;
        [SerializeField] protected TextMeshProUGUI hotKeyTextUI;
        

        public void AssignAction(DataIndex actionCode)
        {
            
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            skillObjectHierarchy =   transform.Find("ActionHierarchy");
            hotKeyTextUI         ??= GetComponentInChildren<TextMeshProUGUI>();
            hotKeyTextUI.text = hotKey.ToString() == "None" 
                ? string.Empty 
                : hotKey.ToString();
        }
#endif
    }
}
