using Common;
using Common.Execution;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Warrior.Skills
{
    public class Finisher : SkillComponent
    {
        [SerializeField] private float remainBonusMultiplier = 1f;
        
        private DamageExecutor damageExecutor;
        
        public override void Initialize()
        {
            base.Initialize();

            damageExecutor = GetComponentInChildren<DamageExecutor>();
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", MultipliedDamageExecution);
        }


        private void MultipliedDamageExecution()
        {
            var remainResource = Cb.DynamicStatEntry.Resource.Value;
            var maxResource = Cb.StatTable.MaxResource;
            var multiplier = 1.0f + ((remainResource / maxResource) * remainBonusMultiplier);
            var originalPower = damageExecutor.DamageSpec.Power;
            
            damageExecutor.DamageSpec.Change(StatType.Power, originalPower * multiplier);
            detector.GetTakers()?.ForEach(executor.Execute);
            damageExecutor.DamageSpec.Change(StatType.Power, originalPower);
            Cb.DynamicStatEntry.Resource.Value = 0f;
        }
    }
}
