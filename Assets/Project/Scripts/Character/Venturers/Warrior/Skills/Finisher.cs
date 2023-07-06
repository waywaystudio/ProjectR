using Common;
using Common.Execution;
using Common.Execution.Variants;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Warrior.Skills
{
    public class Finisher : SkillComponent
    {
        [SerializeField] private float remainBonusMultiplier = 1f;
        
        private DamageExecution damageExecutor;
        public StatSpec CombatSpec => damageExecutor.DamageSpec;
        
        public override void Initialize()
        {
            base.Initialize();

            damageExecutor = GetComponentInChildren<DamageExecution>();
            
            Builder.Add(Section.Execute, "CommonExecution", MultipliedDamageExecution);
        }


        private void MultipliedDamageExecution()
        {
            var remainResource = Cb.Resource.Value;
            var maxResource = Cb.StatTable.MaxResource;
            var multiplier = 1.0f + remainResource / maxResource * remainBonusMultiplier;
            var originalPower = CombatSpec.Power;
            
            CombatSpec.Change(StatType.Power, originalPower * multiplier);
            detector.GetTakers()?.ForEach(executor.ToTaker);
            CombatSpec.Change(StatType.Power, originalPower);
            Cb.Resource.Value = 0f;
        }
    }
}
