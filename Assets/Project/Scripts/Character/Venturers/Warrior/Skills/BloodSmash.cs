using Common.Skills;

namespace Character.Venturers.Warrior.Skills
{
    public class BloodSmash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder.Add(Section.Execute, "BloodSmashAttack", BloodSmashAttack);
        }


        private void BloodSmashAttack()
        {
            detector.GetTakers()?.ForEach(Invoker.Hit);
        }
    }
}
