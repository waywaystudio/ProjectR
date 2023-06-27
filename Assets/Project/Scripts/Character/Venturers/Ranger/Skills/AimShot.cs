using Common;
using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class AimShot : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Provider.OnDamageProvided.Add("AddAdrenalinByAimShot", AddAdrenalin);
            SequenceBuilder.Add(SectionType.Execute, "ShotAttackExecution", () => executor.Execute(null))
                           .Add(SectionType.Execute, "PlayCastCompleteAnimation", PlayCastCompleteAnimation);

        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            Provider.OnDamageProvided.Remove("AddAdrenalinByInstantShot");
        }
        
        
        private void PlayCastCompleteAnimation()
        {
            Cb.Animating.PlayOnce("AimHoldFire", 1f + Haste, SkillInvoker.Complete);
        }
        
        private void AddAdrenalin(CombatEntity damageLog)
        {
            if (damageLog.CombatIndex != DataIndex) return;
            if (damageLog.IsCritical)
            {
                executor.Execute(ExecuteGroup.Group2, Cb);
            }
            else
            {
                if (!Cb.DynamicStatEntry.StatusEffectTable
                       .TryGetValue(DataIndex.AdrenalinStatusEffect, out var statusEffect)) return;
                
                statusEffect.Dispel();
            }
        }
    }
}
