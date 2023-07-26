using Common;
using Common.Characters;
using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class InstantShot : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Provider.OnCombatProvided.Add("AddAdrenalinByInstantShot", AddAdrenalin);
            Builder.Add(Section.Execute, "Fire", Fire);
        }
        

        protected override void Dispose()
        {
            base.Dispose();
            
            Provider.OnCombatProvided.Remove("AddAdrenalinByInstantShot");
        }

        private void Fire()
        {
            var forwardPosition = Provider.Position + Provider.Forward;

            Invoker.Fire(forwardPosition);
        }

        private void AddAdrenalin(CombatLog damageLog)
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
