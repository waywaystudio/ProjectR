using Character.Adventurers;
using Common.Characters.Behaviours;
using Common.Skills;
using Common.UI;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    public class CastingProgress : MonoBehaviour
    {
        [SerializeField] private ImageFiller progress;

        private SkillBehaviour characterAction;
        private SkillComponent currentSkill;

        public void Initialize()
        {
            characterAction = RaidDirector.FocusCharacter.SkillBehaviour;
            characterAction.OnExecuting.Unregister("ShowCastingUI");
            characterAction.OnExecuting.Register("ShowCastingUI", Register);
        }

        public void OnAdventurerChanged(Adventurer ab)
        {
            Unregister();

            if (ab.IsNullOrEmpty()) return;
            
            characterAction = ab.SkillBehaviour;
            characterAction.OnExecuting.Register("ShowCastingUI", Register);

            Register();
        }

        private void Unregister()
        {
            characterAction.OnExecuting.Unregister("ShowCastingUI");
            characterAction.SkillList.ForEach(x => x.OnEnded.Unregister("OffCastingUI"));
        }

        private void Register()
        {
            currentSkill = characterAction.Current;
            
            if (currentSkill.IsNullOrEmpty() || currentSkill.CastingProgress.Value == 0f)
            {
                progress.Unregister();
                progress.gameObject.SetActive(false);
                return;
            }
            
            // Image On
            progress.gameObject.SetActive(true);

            // FillImage
            progress.Register(currentSkill.CastingProgress, currentSkill.ProgressTime);            
            currentSkill.OnEnded.Register("OffCastingUI", () => progress.gameObject.SetActive(false));
        }
    }
}
