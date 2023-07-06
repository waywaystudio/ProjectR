using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class Slash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            cost.PayCondition.Add("HasTarget", detector.HasTarget);
            Builder
                .Add(SectionType.Execute, "SlashAttack", SlashAttack);
        }


        private void SlashAttack()
        {
            detector.GetTakers()?.ForEach(taker =>
            {
                // Taker = taker;
                executor.ToTaker(taker);
            });
        }
    }
}

