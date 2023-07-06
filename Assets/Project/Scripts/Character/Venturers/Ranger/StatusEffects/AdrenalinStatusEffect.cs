using Common;
using Common.StatusEffects;
using Sirenix.OdinInspector;

namespace Character.Venturers.Ranger.StatusEffects
{
    public class AdrenalinStatusEffect : StatusEffect
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(Section.Override, "GetHuntersEcstasy", GetHuntersEcstasy);
        }

        
        private void GetHuntersEcstasy()
        {   
            executor.ToTaker(Taker);
            Dispel();
        }
    }
}
