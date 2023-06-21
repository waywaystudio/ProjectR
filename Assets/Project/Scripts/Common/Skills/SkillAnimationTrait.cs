using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillAnimationTrait
    {
        [SerializeField] private string animationKey;
        [SerializeField] private bool isLoop;
        [SerializeField] private bool hasEvent;
        [SerializeField] private SkillType skillType;
        [SerializeField] private SectionType callbackSection = SectionType.Complete;

        public SkillType SkillType => skillType;
        public float TimeScale { get; private set; } = 0f;

        public void Initialize(SkillComponent skill)
        {
            var animator = skill.Cb.Animating;
            var callback = callbackSection.GetInvokeAction(skill);
            
            // TODO.Converting...
            if (skill.Cb.CombatClass == CharacterMask.Villain)
            {
                skill.SequenceBuilder
                     .Add(SectionType.Active, "PlayAnimator", () => animator.Play(animationKey, 1f, callback));
            }
            else
            {
                skill.SequenceBuilder
                     .Add(SectionType.Active,"PlayAnimation",
                          () => animator.Play(animationKey, TimeScale, callback));
            }

            if (hasEvent)
            {
                skill.SequenceBuilder
                     .Add(SectionType.Active,"RegisterHitEvent", () => animator.OnHitEventTable.Add("SkillHit", skill.SkillInvoker.Execute))        
                     .Add(SectionType.End,"ReleaseHit", () => animator.OnHitEventTable.Remove("SkillHit"));
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
