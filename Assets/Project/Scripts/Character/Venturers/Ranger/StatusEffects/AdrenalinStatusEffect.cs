using Common;
using Common.StatusEffects;
using Sirenix.OdinInspector;

namespace Character.Venturers.Ranger.StatusEffects
{
    public class AdrenalinStatusEffect : StatusEffect
    {
        [ShowInInspector]
        private int stack;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Active, "AddStack", () => stack++);
        }

        public override void Overriding()
        {
            // TODO.TEST
            stack++;
            
            // Add Hunter's Ecstasy Effect
            // Taker == Self
            executor.Execute(Taker);

            // Remove Adrenalin
            Dispel();
        }
    }
}
