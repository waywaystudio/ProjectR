using Character.Adventurers;
using Common.Actions;
using Common.Skills;
using Common.UI;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    public class CastingProgress : MonoBehaviour
    {
        [SerializeField] private ImageFiller progress;

        private ActionBehaviour characterAction;
        private SkillComponent currentSkill;

        public void Initialize()
        {
            characterAction = RaidDirector.FocusCharacter.CharacterAction;
            characterAction.SkillAction.OnGlobalActivated.Unregister("ShowCastingUI");
            characterAction.SkillAction.OnGlobalActivated.Register("ShowCastingUI", Register);
        }

        public void OnAdventurerChanged(Adventurer ab)
        {
            Unregister();

            if (ab.IsNullOrEmpty()) return;
            
            characterAction = ab.CharacterAction;
            characterAction.SkillAction.OnGlobalActivated.Register("ShowCastingUI", Register);

            Register();
        }

        private void Unregister()
        {
            characterAction.SkillAction.OnGlobalActivated.Unregister("ShowCastingUI");
            characterAction.SkillList.ForEach(x => x.OnEnded.Unregister("OffCastingUI"));
        }

        private void Register()
        {
            currentSkill = characterAction.SkillAction.Current;
            
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
