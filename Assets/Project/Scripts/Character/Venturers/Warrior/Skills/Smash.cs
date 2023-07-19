using Common.Skills;

namespace Character.Venturers.Warrior.Skills
{
    public class Smash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .Add(Section.Execute, "SmashAttack",SmashAttack);
        }


        private void SmashAttack()
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
