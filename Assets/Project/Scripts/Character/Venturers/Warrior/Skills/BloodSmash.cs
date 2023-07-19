using Common.Characters;
using Common.Skills;

namespace Character.Venturers.Warrior.Skills
{
    public class BloodSmash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .Add(Section.Execute, "BloodSmashAttack", BloodSmashAttack);
        }


        private void BloodSmashAttack()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
    }
}
