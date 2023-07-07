using Common;
using Common.Particles;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Skills
{
    public class Bash : SkillComponent
    {
        [SerializeField] private SinglePool<ParticleComponent> counterParticle;
        [SerializeField] private float counterAttackBonus = 8;
        
        public override void Initialize()
        {
            base.Initialize();
            
            counterParticle.Initialize(null, transform);

            Builder
                .Add(Section.Execute, "BashAttack", BashAttack)
                .Add(Section.Extra, "ExtraBonus", ExtraBonus)
                .Add(Section.Extra, "PlayExtraParticle", PlayExtraParticle);
        }


        private void BashAttack()
        {
            var getBonus = false;
            
            detector.GetTakers()?.ForEach(taker =>
            {
                executor.ToTaker(taker);

                if (getBonus || taker.BehaviourMask != ActionMask.Skill) return;
                
                getBonus = true;
                Sequencer[Section.Extra].Invoke();
            });
        }
        
        // 대상이 Skill 시전 중에 Bash 타격 시 추가 자원 획득.
        private void ExtraBonus()
        {
            Cb.Resource.Value += counterAttackBonus;
        }

        private void PlayExtraParticle()
        {
            counterParticle.Get().Play();
        }
    }
}
