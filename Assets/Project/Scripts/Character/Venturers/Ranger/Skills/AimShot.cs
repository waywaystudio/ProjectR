using Common;
using Common.Characters;
using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class AimShot : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Provider.OnDamageProvided.Add("AddAdrenalinByAimShot", AddAdrenalin);
            Builder
                .Add(Section.Execute, "Fire", Fire);

        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            Provider.OnDamageProvided.Remove("AddAdrenalinByInstantShot");
        }
        
        
        private void Fire()
        {
            var forwardPosition = Provider.Position + Provider.Forward;

            Invoker.Fire(forwardPosition);
        }

        private void AddAdrenalin(CombatEntity damageLog)
        {
            if (damageLog.CombatIndex != DataIndex) return;
            if (damageLog.IsCritical)
            {
                Invoker.SubHit(Cb);
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
