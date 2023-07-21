using Common;
using Common.Execution.Hits;
using Common.Projectors;
using Common.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Venturers.Warrior.Skills
{
    public class Finisher : SkillComponent
    {
        [FormerlySerializedAs("projector")] [SerializeField] private Projection projection;
        [SerializeField] private DamageHit damageHit;
        [SerializeField] private float remainBonusMultiplier = 1f;

        public StatSpec CombatSpec => damageHit.DamageSpec;
        
        public override void Initialize()
        {
            base.Initialize();

            projection.Initialize(this);
            Builder
                .Add(Section.Execute, "CommonExecution", MultipliedDamageExecution);
        }


        private void MultipliedDamageExecution()
        {
            var remainResource = Cb.Resource.Value;
            var maxResource = Cb.StatTable.MaxResource;
            var multiplier = 1.0f + (1.0f - remainResource / maxResource) * remainBonusMultiplier;
            var originalPower = CombatSpec.Power;
            
            CombatSpec.Change(StatType.Power, originalPower * multiplier);
            
            if (detector.TryGetTakers(out var takers))
            {
                takers.ForEach(taker =>
                {
                    Taker = taker;
                    Invoker.Hit(taker);
                });
            }

            CombatSpec.Change(StatType.Power, originalPower);
            Cb.Resource.Value = 0f;
        }
    }
}
