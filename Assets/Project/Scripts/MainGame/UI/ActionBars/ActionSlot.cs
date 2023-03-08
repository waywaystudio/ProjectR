using Core;
using MainGame.UI.ImageUtility;
using TMPro;
using UnityEngine;

namespace MainGame.UI.ActionBars
{
    public class ActionSlot : MonoBehaviour, IEditable
    {
        [SerializeField] protected BindingCode hotKey;
        [SerializeField] protected Transform skillObjectHierarchy;
        [SerializeField] protected TextMeshProUGUI hotKeyTextUI;
        // [SerializeField] protected ImageFiller filler;
        

        public void AssignAction(DataIndex actionCode)
        {
            
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            skillObjectHierarchy =   transform.Find("ActionHierarchy");
            hotKeyTextUI         ??= GetComponentInChildren<TextMeshProUGUI>();
            // filler               ??= transform.Find("SkillCooldown").GetComponent<ImageFiller>();

            hotKeyTextUI.text = hotKey.ToString() == "None" 
                ? "#" 
                : hotKey.ToString();
        }
#endif
    }
}
