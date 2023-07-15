using System;
using Common.Animation;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillAnimationTrait
    {
        [SerializeField] private string animationKey;
        [SerializeField] private string castCompleteAnimationKey;
        [SerializeField] private bool isLoop;
        [SerializeField] private bool hasExecuteEvent;
        [SerializeField] private float animationPlaySpeed = 1.0f;
        [SerializeField] private Section callbackSection = Section.Complete;

        private float AnimationPlaySpeed => HasteRetriever is null 
            ? animationPlaySpeed 
            : animationPlaySpeed * HasteRetriever.Invoke();
        
        private Func<float> HasteRetriever  { get; set; }
        private AnimationModel Model { get; set; }


        public void Initialize(SkillComponent skill)
        {
            Model          =  skill.Cb.Animating;
            HasteRetriever += () => 1.0f + skill.Haste.Invoke();
            
            var hasCasting = skill.SkillType is SkillType.Charging or SkillType.Holding or SkillType.Casting;
            var animationCallback = callbackSection.GetCombatInvoker(skill.Invoker); 

            skill.Builder
                 .Add(Section.Active, "PlayAnimation", () => PlayAnimation(animationCallback))
                 .AddIf(hasExecuteEvent, Section.Active, "RegisterHitEvent", () => Model.OnHit.Add("SkillHit", skill.Invoker.Execute))
                 .AddIf(hasExecuteEvent, Section.End,"ReleaseHit", () => Model.OnHit.Remove("SkillHit"))
                 .AddIf(hasCasting, Section.Execute, "CastingCompleteAnimation",() => PlayCompleteCastingAnimation(skill));
        }


        private void PlayAnimation(Action callback)
        {
            if (animationKey == "None")
            {
                callback?.Invoke();
                return;
            }

            Model.Play(animationKey, 0, isLoop, AnimationPlaySpeed, callback);
        }

        private void PlayCompleteCastingAnimation(ICombatObject skill)
        {
            if (castCompleteAnimationKey == "") return;
            
            Model.PlayOnce(castCompleteAnimationKey, AnimationPlaySpeed, skill.Invoker.Complete);
        }

        
        

#if UNITY_EDITOR
        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            animationKey             = skillData.AnimationKey;
            castCompleteAnimationKey = skillData.CastCompleteKey;
            isLoop                   = skillData.IsLoop;
            hasExecuteEvent          = skillData.HasEvent;
            callbackSection          = skillData.AnimationCallback.ToEnum<Section>();
        }
#endif
    }
}
