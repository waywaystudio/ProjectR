using Character;
using Character.Actions;
using Character.Skill;
using Core;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    public class CastingProgress : MonoBehaviour
    {
        [SerializeField] private ImageFiller progress;

        private ActionBehaviour actionBehaviour;
        private SkillComponent currentSkill;

        public void Initialize()
        {
            actionBehaviour = RaidDirector.FocusCharacter.ActionBehaviour;
            actionBehaviour.OnGlobalActivated.Unregister("ShowCastingUI");
            actionBehaviour.OnGlobalActivated.Register("ShowCastingUI", Register);
        }

        public void OnAdventurerChanged(Adventurer ab)
        {
            Unregister();

            if (ab.IsNullOrEmpty()) return;
            
            actionBehaviour = ab.ActionBehaviour;
            actionBehaviour.OnGlobalActivated.Register("ShowCastingUI", Register);

            Register();
        }

        private void Unregister()
        {
            actionBehaviour.OnGlobalActivated.Unregister("ShowCastingUI");
            actionBehaviour.SkillList.ForEach(x => x.OnEnded.Unregister("OffCastingUI"));
        }

        private void Register()
        {
            currentSkill = actionBehaviour.Current;
            
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
