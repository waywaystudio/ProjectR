using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Skills
{
    public class Bash : SkillComponent
    {
        [SerializeField] private float counterAttackBonus = 8;
        
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.Execute))
                           .Add(SectionType.Execute,"BashTimingBonus", OnSuccess);

        }
        
        // 대상이 Skill 시전 중에 Bash 타격 시 추가 자원 획득.
        private void OnSuccess()
        {
            var mainTarget = detector.GetMainTarget();

            if (mainTarget is null) return;
            if (mainTarget.BehaviourMask == ActionMask.Skill)
            {
                Cb.DynamicStatEntry.Resource.Value += counterAttackBonus;
            }
        }
    }
}
