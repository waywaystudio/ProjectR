using Common.UI;
using UnityEngine;

namespace Common.Characters.UI.CastingFrames
{
    public class CastingFrame : MonoBehaviour
    {
        [SerializeField] private GameObject castingObject;
        [SerializeField] private ImageFiller castingBar;

        private CharacterBehaviour cb;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        private void Show()
        {
            var currentSkill = Cb.SkillBehaviour.Current;

            if (currentSkill.CastTimer.CastingTime == 0)
            {
                castingBar.Unregister();
                castingObject.SetActive(false);
                return;
            }

            castingObject.gameObject.SetActive(true);
            castingBar.Register(currentSkill.CastTimer.EventTimer, currentSkill.CastTimer.CastingTime);

            currentSkill.SequenceBuilder.Add(SectionType.End,"HideCastingUI", Hide);
        }
        
        private void Hide()
        {
            castingObject.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            // Cb.SkillBehaviour.SequenceBuilder.AddActive("ShowCastingUI", Show);
        }

        private void OnDisable()
        {
            // Cb.SkillBehaviour.SequenceBuilder.RemoveActive("ShowCastingUI");
        }
    }
}
