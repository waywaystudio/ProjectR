namespace Character.Combat.Skill
{
    public class ChainHealing : SkillObject
    {
        protected override void CompleteSkill()
        {
            base.CompleteSkill();

            if (TargetModule && ProjectileModule)
                ProjectileModule.Fire(TargetModule.Target);
        }
    }
}
