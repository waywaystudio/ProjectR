using Common.Skills;
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


        private void OnEnable()
        {
            coolDownFiller.ProgressImage.enabled = false;
            
            if (Cb.IsNullOrDestroyed()) return;
            if (Cb.GetSkill(actionCode).TryGetComponent(out CoolTimer coolTime))
            {
                coolDownFiller.ProgressImage.enabled = true;
                coolDownFiller.Register(coolTime.RemainCoolTime, coolTime.CoolTime);
            }
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

            actionIcon.sprite = Cb.GetSkill(actionCode).Icon;
        }

        public void EditorPersonalSetUp(DataIndex actionCode)
        {
            this.actionCode = actionCode;

            EditorSetUp();
        }
#endif
    }
}
