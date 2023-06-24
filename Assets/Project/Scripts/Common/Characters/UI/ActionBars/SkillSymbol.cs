using Common.UI;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Common.Characters.UI.ActionBars
{
    public class SkillSymbol : MonoBehaviour, IEditable
    {
        [SerializeField] private Image actionIcon;
        [SerializeField] private ImageFiller coolDownFiller;
        [SerializeField] private DataIndex actionCode;

        private CharacterBehaviour cb;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        public void StartAction(InputAction.CallbackContext callbackContext)
        {
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;

            Cb.ExecuteSkill(actionCode, mousePosition);
        }

        public void ReleaseAction(InputAction.CallbackContext callbackContext)
        {
            Cb.ReleaseSkill();
        }

        public void UpdateSymbol(DataIndex dataIndex)
        {
            if (dataIndex == actionCode) return;

            actionCode = dataIndex;
            var targetSkill = Cb.GetSkill(actionCode);
            if (targetSkill.IsNullOrEmpty()) return;

            actionIcon.sprite = targetSkill.Icon;
            
            var coolTimer = targetSkill.CoolTimer;
            coolDownFiller.Register(coolTimer.EventTimer, coolTimer.CoolTime);
        }


        private void OnEnable()
        {
            coolDownFiller.ProgressImage.enabled = false;
            
            if (Cb.IsNullOrDestroyed()) return;
            
            var coolTimer = Cb.GetSkill(actionCode).CoolTimer;

            if (coolTimer.CoolTime == 0f) return;
            
            coolDownFiller.ProgressImage.enabled = true;
            coolDownFiller.Register(coolTimer.EventTimer, coolTimer.CoolTime);
        }

        private void OnDisable()
        {
            coolDownFiller.Unregister();
            
            if (Cb.IsNullOrDestroyed()) return;
            cb = null;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            actionIcon     = GetComponent<Image>();
            coolDownFiller = transform.Find("Cooldown").GetComponent<ImageFiller>();

            if (Cb.IsNullOrDestroyed()) return;
            
            var targetSkill = Cb.GetSkill(actionCode);
            if (targetSkill.IsNullOrEmpty()) return;

            actionIcon.sprite = targetSkill.Icon;
        }

        public void EditorPersonalSetUp(DataIndex actionCode)
        {
            this.actionCode = actionCode;

            EditorSetUp();
        }
#endif
    }
}
