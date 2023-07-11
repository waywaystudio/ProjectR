using Common;
using Common.Execution.Hits;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Warrior.Skills
{
    public class Finisher : SkillComponent
    {
        [SerializeField] private float remainBonusMultiplier = 1f;
        
        private DamageHit damageExecutor;
        public StatSpec CombatSpec => damageExecutor.DamageSpec;
        
        public override void Initialize()
        {
            base.Initialize();

            damageExecutor = GetComponentInChildren<DamageHit>();
            
            Builder.Add(Section.Execute, "CommonExecution", MultipliedDamageExecution);
        }


        private void MultipliedDamageExecution()
        {
            var remainResource = Cb.Resource.Value;
            var maxResource = Cb.StatTable.MaxResource;
            var multiplier = 1.0f + remainResource / maxResource * remainBonusMultiplier;
            var originalPower = CombatSpec.Power;
            
            CombatSpec.Change(StatType.Power, originalPower * multiplier);
            detector.GetTakers()?.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
            CombatSpec.Change(StatType.Power, originalPower);
            Cb.Resource.Value = 0f;
        }
    }
}
