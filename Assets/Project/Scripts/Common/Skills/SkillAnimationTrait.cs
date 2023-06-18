using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillAnimationTrait
    {
        [SerializeField] private string animationKey;
        [SerializeField] private float timeScale;
        [SerializeField] private bool isLoop;
        [SerializeField] private bool hasEvent;
        [SerializeField] private SkillType skillType;
        [SerializeField] private SectionType callbackSection = SectionType.Complete;

        public string Key { get => animationKey; set => animationKey = value; }
        public float TimeScale { get => timeScale; set => timeScale = value; }
        public SkillType SkillType { get => skillType; set => skillType = value; }

        public void Initialize(SkillComponent skill)
        {
            var animator = skill.Cb.Animating;
            var callback = callbackSection.GetInvokeAction(skill);
            
            skill.SequenceBuilder
                 .Add(SectionType.Active,"PlayAnimation",
                      () => animator.Play(animationKey, 0, isLoop, timeScale, callback));

            if (hasEvent)
            {
                skill.SequenceBuilder
                     .Add(SectionType.Active,"RegisterHitEvent", () => animator.OnHit.Add("SkillHit", skill.SkillInvoker.Execute))        
                     .Add(SectionType.End,"ReleaseHit", () => animator.OnHit.Remove("SkillHit"));
            }
        }
    }
}
