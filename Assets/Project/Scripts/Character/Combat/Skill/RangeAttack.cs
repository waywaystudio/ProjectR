namespace Character.Combat.Skill
{
    public class RangeAttack : SkillObject
    {
        protected override void CompleteSkill()
        {
            base.CompleteSkill();

            if (TargetModule && ProjectileModule)
            {
                TargetModule.TakeProjectile(ProjectileModule);
            }
        }
    }
}
