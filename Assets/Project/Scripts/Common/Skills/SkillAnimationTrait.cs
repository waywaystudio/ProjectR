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
        [SerializeField] private SkillType skillType;
        [SerializeField] private SectionType callbackSection = SectionType.Complete;

        public SkillType SkillType => skillType;
        public float AnimationPlaySpeed => HasteRetriever is null 
            ? animationPlaySpeed 
            : animationPlaySpeed * HasteRetriever.Invoke();
        
        private Func<float> HasteRetriever  { get; set; }
        private AnimationModel Model { get; set; }


        public void Initialize(SkillComponent skill)
        {
            Model          =  skill.Cb.Animating;
            HasteRetriever += () => 1.0f + skill.Haste;
            
            skill.Builder
                 .Add(SectionType.Active,"PlayAnimation",() => PlayAnimation(skill));

            if (hasExecuteEvent)
            {
                skill.Builder
                     .Add(SectionType.Active,"RegisterHitEvent", () => Model.OnHit.Add("SkillHit", skill.Invoker.Execute))        
                     .Add(SectionType.End,"ReleaseHit", () => Model.OnHit.Remove("SkillHit"));
            }

            if (skillType is SkillType.Casting or SkillType.Charging or SkillType.Holding)
            {
                skill.Builder
                     .Add(SectionType.Execute, "CastingCompleteAnimation",() => PlayCompleteCastingAnimation(skill));
            }
        }


        private void PlayAnimation(SkillComponent skill)
        {
            var callback = callbackSection.GetInvokeAction(skill);

            if (animationKey == "None")
            {
                callback?.Invoke();
                return;
            }

            Model.Play(animationKey, 0, isLoop, AnimationPlaySpeed, callback);
        }

        private void PlayCompleteCastingAnimation(SkillComponent skill)
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
            skillType                = skillData.SkillType.ToEnum<SkillType>();
            callbackSection          = skillData.AnimationCallback.ToEnum<SectionType>();
        }
#endif
    }
}
