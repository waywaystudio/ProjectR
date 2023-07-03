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

            Builder
                .Add(SectionType.Execute, "BashAttack", BashAttack)
                .Add(SectionType.Execute,"BashTimingBonus", BashTimingBonus);

        }


        private void BashAttack()
        {
            detector.GetTakers()?.ForEach(executor.ToTaker);
        }
        
        // 대상이 Skill 시전 중에 Bash 타격 시 추가 자원 획득.
        private void BashTimingBonus()
        {
            var mainTarget = detector.GetMainTarget();

            if (mainTarget is null) return;
            if (mainTarget.BehaviourMask == ActionMask.Skill)
            {
                Cb.Resource.Value += counterAttackBonus;
            }
        }
    }
}
