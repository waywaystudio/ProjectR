namespace Character.Combat.Skill
{
    public class FaithfulPrey : SkillObject
    {
        protected override void CompleteSkill()
        {
            if (HealModule && TargetModule)
            {
                TargetModule.TakeHeal(HealModule);
            }

            base.CompleteSkill();
        }
    }
}
