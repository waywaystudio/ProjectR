using Common;
using Common.Characters;
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
                .Add(Section.Execute, "BashAttack", BashAttack)
                .Add(Section.Extra, "ExtraBonus", ExtraBonus)
                ;
        }


        private void BashAttack()
        {
            var getBonus = false;

            if (!detector.TryGetTakers(out var takers)) return;
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(Taker);

                if (getBonus || taker.BehaviourMask != ActionMask.Skill) return;
                
                getBonus = true;
                Invoker.ExtraAction();
            });
        }
        
        // 대상이 Skill 시전 중에 Bash 타격 시 추가 자원 획득.
        private void ExtraBonus()
        {
            Cb.Resource.Value += counterAttackBonus;
        }
    }
}
