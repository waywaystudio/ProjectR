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
        [SerializeField] private bool hasEvent;
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
            var callback = callbackSection.GetInvokeAction(skill);

            HasteRetriever += () => 1.0f + skill.Haste;
            
            skill.SequenceBuilder
                 .Add(SectionType.Active,"PlayAnimation",
                      () => animator.Play(animationKey, 0, isLoop, AnimationPlaySpeed, callback));

            if (hasEvent)
            {
                skill.SequenceBuilder
                     .Add(SectionType.Active,"RegisterHitEvent", () => animator.OnHit.Add("SkillHit", skill.SkillInvoker.Execute))        
                     .Add(SectionType.End,"ReleaseHit", () => animator.OnHit.Remove("SkillHit"));
            }
        }
        

#if UNITY_EDITOR
        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            animationKey    = skillData.AnimationKey;
            isLoop          = skillData.IsLoop;
            hasEvent        = skillData.HasEvent;
            skillType       = skillData.SkillType.ToEnum<SkillType>();
            callbackSection = skillData.AnimationCallback.ToEnum<SectionType>();
        }
#endif
    }
}
