using Common.Skills;
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
            
            if (currentSkill.IsNullOrEmpty() || !currentSkill.TryGetComponent(out SkillCasting process))
            {
                castingBar.Unregister();
                castingObject.SetActive(false);
                return;
            }
            
            castingObject.gameObject.SetActive(true);
            castingBar.Register(process.Progress, process.CastingTime);

            currentSkill.Sequencer.EndAction.Add("HideCastingUI", Hide);
        }
        
        private void Hide()
        {
            castingObject.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Cb.SkillBehaviour.Sequencer.ActiveAction.Add("ShowCastingUI", Show);
        }

        private void OnDisable()
        {
            Cb.SkillBehaviour.Sequencer.ActiveAction.Remove("ShowCastingUI");
        }
    }
}
