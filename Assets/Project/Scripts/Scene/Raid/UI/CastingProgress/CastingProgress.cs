using System;
using Character;
using Character.Skill;
using Core;
using UnityEngine;

namespace Raid.UI.CastingProgress
{
    public class CastingProgress : MonoBehaviour
    {
        [SerializeField] private ImageFiller progress;

        private AdventurerBehaviour focusAdventurer;
        private ActionBehaviour actionBehaviour;
        private SkillComponent currentSkill;

        public void Initialize(AdventurerBehaviour ab)
        {
            focusAdventurer = ab;
            
            actionBehaviour = ab.ActionBehaviour;
            actionBehaviour.OnActivatedSkill.Unregister("ShowCastingUI");
            actionBehaviour.OnActivatedSkill.Register("ShowCastingUI", Register);
        }

        public void OnAdventurerChanged(AdventurerBehaviour ab)
        { 
            actionBehaviour.OnActivatedSkill.Unregister("ShowCastingUI");
            actionBehaviour.SkillList.ForEach(x => x.OnEnded.Unregister("OffCastingUI"));

            actionBehaviour = ab.ActionBehaviour;
            actionBehaviour.OnActivatedSkill.Register("ShowCastingUI", Register);

            Register();
        }

        public void Register()
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
