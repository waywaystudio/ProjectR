namespace Character.Combat.Skill
{
    public class FaithfulPrey : SkillObject
    {
        protected override void CompleteSkill()
        {
            if (HealModule && TargetModule)
                TargetModule.Target.TakeHeal(HealModule);

            base.CompleteSkill();
        }
    }
}
