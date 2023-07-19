using Common.Characters;
using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class Slash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .Add(Section.Execute, "SlashAttack", SlashAttack);
        }


        private void SlashAttack()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            SkillCost.PayCost(Cb.Resource);
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
    }
}

