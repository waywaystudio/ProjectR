using Common.Skills;

namespace Character.Venturers.Warrior.Skills
{
    public class BloodSmash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder.Add(SectionType.Execute, "BloodSmashAttack", BloodSmashAttack);
        }


        private void BloodSmashAttack()
        {
            detector.GetTakers()?.ForEach(executor.ToTaker);
        }
    }
}
