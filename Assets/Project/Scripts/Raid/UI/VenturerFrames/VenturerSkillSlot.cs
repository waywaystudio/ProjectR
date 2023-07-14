using Character.Venturers;
using Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI.VenturerFrames
{
    public class VenturerSkillSlot : MonoBehaviour, IEditable
    {
        [SerializeField] private string bindingKey;
        [SerializeField] private Image skillIcon;
        [SerializeField] private ImageFiller coolDownFiller;
        [SerializeField] private TextMeshProUGUI hotKey;

        private static VenturerBehaviour FocusVenturer => RaidDirector.FocusVenturer;
        private DataIndex SkillCode { get; set; }
        private string HotKey => bindingKey;


        public void Initialize()
        {
            RaidDirector.Input[bindingKey]?.AddStart("ActiveSkill", StartAction);
            RaidDirector.Input[bindingKey]?.AddCancel("ReleaseSkill", ReleaseAction);
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


        private void StartAction(InputAction.CallbackContext callbackContext)
        {
            if (!InputManager.TryGetMousePosition(out var mousePosition)) return;

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
            skillIcon      = transform.Find("Icon").GetComponent<Image>();
            coolDownFiller = transform.Find("Cooldown").GetComponent<ImageFiller>();
            hotKey.text    = HotKey;
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
