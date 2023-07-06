using Common;
using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class InstantShot : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Provider.OnDamageProvided.Add("AddAdrenalinByInstantShot", AddAdrenalin);
            Builder.Add(Section.Execute, "Fire", Fire);
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
