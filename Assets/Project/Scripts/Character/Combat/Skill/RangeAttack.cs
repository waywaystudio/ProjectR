namespace Character.Combat.Skill
{
    public class RangeAttack : SkillObject
    {
        protected override void CompleteSkill()
        {
            base.CompleteSkill();

            if (TargetModule && ProjectileModule)
                ProjectileModule.Fire(TargetModule.Target);
        }
    }
}
