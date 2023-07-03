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
            Builder.Add(SectionType.Execute, "Fire", Fire)
                           .Add(SectionType.Execute, "PlayCastCompleteAnimation", PlayCastCompleteAnimation);

        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            Provider.OnDamageProvided.Remove("AddAdrenalinByInstantShot");
        }
        
        
        private void Fire()
        {
            var forwardPosition = Provider.Position + Provider.Forward;

            executor.ToPosition(forwardPosition);
        }
        
        private void PlayCastCompleteAnimation()
        {
            Cb.Animating.PlayOnce("AimHoldFire", 1f + Haste, Invoker.Complete);
        }
        
        private void AddAdrenalin(CombatEntity damageLog)
        {
            if (damageLog.CombatIndex != DataIndex) return;
            if (damageLog.IsCritical)
            {
                executor.ToTaker(Cb, ExecuteGroup.Group2);
            }
            else
            {
                if (!Cb.StatusEffectTable
                       .TryGetValue(DataIndex.AdrenalinStatusEffect, out var statusEffect)) return;
                
                statusEffect.Dispel();
            }
        }
    }
}
