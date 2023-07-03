using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Skills
{
    [Serializable]
    public class SkillAnimationTrait
    {
        [SerializeField] private string animationKey;
        [SerializeField] private bool isLoop;
        [FormerlySerializedAs("hasEvent")] [SerializeField] private bool hasExecuteEvent;
        [SerializeField] private float animationPlaySpeed = 1.0f;
        [SerializeField] private SkillType skillType;
        [SerializeField] private SectionType callbackSection = SectionType.Complete;

        public SkillType SkillType => skillType;
        public float AnimationPlaySpeed => HasteRetriever is null 
            ? animationPlaySpeed 
            : animationPlaySpeed * HasteRetriever.Invoke();
        
        private Func<float> HasteRetriever  { get; set; }
        

        public void Initialize(SkillComponent skill)
        {
            var animator = skill.Cb.Animating;

            HasteRetriever += () => 1.0f + skill.Haste;
            
            skill.Builder
                 .Add(SectionType.Active,"PlayAnimation",() => PlayAnimation(skill));

            if (hasExecuteEvent)
            {
                skill.Builder
                     .Add(SectionType.Active,"RegisterHitEvent", () => animator.OnHit.Add("SkillHit", skill.Invoker.Execute))        
                     .Add(SectionType.End,"ReleaseHit", () => animator.OnHit.Remove("SkillHit"));
            }
        }


        private void PlayAnimation(SkillComponent skill)
        {
            var animator = skill.Cb.Animating;
            var callback = callbackSection.GetInvokeAction(skill);

            if (animationKey == "None")
            {
                callback?.Invoke();
                return;
            }

            animator.Play(animationKey, 0, isLoop, AnimationPlaySpeed, callback);
        }
        

#if UNITY_EDITOR
        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            animationKey    = skillData.AnimationKey;
            isLoop          = skillData.IsLoop;
            hasExecuteEvent        = skillData.HasEvent;
            skillType       = skillData.SkillType.ToEnum<SkillType>();
            callbackSection = skillData.AnimationCallback.ToEnum<SectionType>();
        }
#endif
    }
}
