using Common;
using Common.StatusEffects;

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
            Invoker.Hit(Taker);
            Dispel();
        }
    }
}
