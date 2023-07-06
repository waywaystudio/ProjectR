using Character.Venturers;
using Common.UI;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI.VenturerFrames
{
    public class VenturerSkillSlot : MonoBehaviour, IEditable
    {
        [SerializeField] private BindingCode bindingCode;
        [SerializeField] private Image skillIcon;
        [SerializeField] private ImageFiller coolDownFiller;
        [SerializeField] private TextMeshProUGUI hotKey;

        private static VenturerBehaviour FocusVenturer => RaidDirector.FocusVenturer;
        private DataIndex SkillCode { get; set; }
        private BindingCode BindingCode => bindingCode;
        private string HotKey => bindingCode switch
        {
            BindingCode.Q => "Q",
            BindingCode.W => "W",
            BindingCode.E => "E",
            BindingCode.R => "R",
            _ => "-",
        };


        public void Initialize()
        {
            if (!MainManager.Input.TryGetAction(BindingCode, out var inputAction))
            {
                Debug.LogWarning($"Not exist InputAction by {BindingCode}");
                return;
            }

            inputAction.started  += StartAction;
            inputAction.canceled += ReleaseAction;
        }

        public void UpdateSlot(DataIndex skillCode)
        {
            SkillCode = skillCode;
            
            var targetSkill = FocusVenturer.GetSkill(skillCode);
            if (targetSkill.IsNullOrEmpty()) return;
            
            skillIcon.sprite = targetSkill.Icon;
            
            var coolTimer = targetSkill.CoolTimer;
            coolDownFiller.Register(coolTimer.EventTimer, coolTimer.CoolTime);
        }

        public void Dispose()
        {
            if (MainManager.Input.IsNullOrEmpty()) return;
            if (!MainManager.Input.TryGetAction(BindingCode, out var inputAction))
            {
                Debug.LogWarning($"Not exist InputAction by {BindingCode}");
                return;
            }

            inputAction.started  -= StartAction;
            inputAction.canceled -= ReleaseAction;
        }
        
        
        private void StartAction(InputAction.CallbackContext callbackContext)
        {
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;

            FocusVenturer.ActiveSkill(SkillCode, mousePosition);
        }

        private void ReleaseAction(InputAction.CallbackContext callbackContext)
        {
            FocusVenturer.ReleaseSkill();
        }

        


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            hotKey         = transform.Find("Hotkey").GetComponent<TextMeshProUGUI>();
            hotKey.text    = HotKey;
            skillIcon      = transform.Find("Icon").GetComponent<Image>();
            coolDownFiller = transform.Find("Cooldown").GetComponent<ImageFiller>();
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
