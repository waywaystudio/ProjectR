using Character.Adventurers;
using Common.Actions;
using Common.Skills;
using Common.UI;
using UnityEngine;

namespace Character.ActionFrames.AdventurerFrame
{
    public class CastingBar : MonoBehaviour
    {
        [SerializeField] private ImageFiller progress;
        
        private Adventurer adventurer;
        
        private ActionBehaviour ActionBehaviour => adventurer.CharacterAction;
        private SkillComponent CurrentSkill => ActionBehaviour.SkillAction.Current;
        
        public void Focus()
        {
            ActionBehaviour.SkillAction.OnGlobalActivated.Register("Progression", Progression);
        }

        public void Release()
        {
            ActionBehaviour.SkillAction.OnGlobalActivated.Unregister("ShowCastingBar");
        }


        private void Progression()
        {
            if (CurrentSkill.IsNullOrEmpty() || CurrentSkill.CastingProgress.Value == 0f)
            {
                progress.Unregister();
                progress.gameObject.SetActive(false);
                return;
            }
            
            // Image On
            progress.gameObject.SetActive(true);
            
            // FillImage
            progress.Register(CurrentSkill.CastingProgress, CurrentSkill.ProgressTime);
            CurrentSkill.OnEnded.Register("OffCastingUI", () => progress.gameObject.SetActive(false));
        }
        

        private void OnEnable()
        {
            adventurer = GetComponentInParent<Adventurer>();
        }

        private void OnDisable()
        {
            Release();
        }
    }
}
