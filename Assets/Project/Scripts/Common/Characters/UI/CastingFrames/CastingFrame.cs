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
            
            if (currentSkill.IsNullOrEmpty() || !currentSkill.TryGetComponent(out SkillProcess process))
            {
                castingBar.Unregister();
                castingObject.SetActive(false);
                return;
            }
            
            castingObject.gameObject.SetActive(true);
            castingBar.Register(process.CastingProgress, process.ProcessTime);
            
            currentSkill.OnCompleted.Register("HideCastingUI", Hide);
            currentSkill.OnCanceled.Register("HideCastingUI", Hide);
        }
        
        private void Hide()
        {
            castingObject.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Cb.SkillBehaviour.OnExecuting.Register("ShowCastingUI", Show);
        }

        private void OnDisable()
        {
            Cb.SkillBehaviour.OnExecuting.Unregister("ShowCastingUI");
        }
    }
}
