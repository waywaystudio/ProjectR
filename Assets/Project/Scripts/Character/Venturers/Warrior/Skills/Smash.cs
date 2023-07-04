using Common.Skills;

namespace Character.Venturers.Warrior.Skills
{
    public class Smash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            cost.PayCondition.Add("HasTarget", detector.HasTarget);

            Builder.Add(SectionType.Execute, "SmashAttack",SmashAttack);
        }


        private void SmashAttack()
        {
            detector.GetTakers()?.ForEach(executor.ToTaker);
        }
    }
}
