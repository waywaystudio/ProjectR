using Character;
using Character.Skill;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid.UI.BossFrames
{
    public class BossCastingProgress : MonoBehaviour
    {
        [SerializeField] private ImageFiller progress;
        
        private ActionBehaviour actionBehaviour;
        private SkillComponent currentSkill;
        
        [ShowInInspector]
        private FloatEvent targetProgress = new();
        
        public void Initialize()
        {
            actionBehaviour = RaidDirector.Boss.ActionBehaviour;
            actionBehaviour.OnCommonActivated.Unregister("ShowCastingUI");
            actionBehaviour.OnCommonActivated.Register("ShowCastingUI", ShowCastingUI);
        }
        
        private void ShowCastingUI()
        {
            currentSkill = actionBehaviour.Current;
            if (currentSkill.IsNullOrEmpty() || currentSkill.CastingProgress.Value == 0f) return;

            progress.Unregister();
            progress.gameObject.SetActive(false);
            
            // Image On
            progress.gameObject.SetActive(true);

            // FillImage
            targetProgress = currentSkill.CastingProgress;
            
            progress.Register(currentSkill.CastingProgress, currentSkill.ProgressTime);            
            currentSkill.OnEnded.Register("OffCastingUI", () => progress.gameObject.SetActive(false));
        }
    }
}
