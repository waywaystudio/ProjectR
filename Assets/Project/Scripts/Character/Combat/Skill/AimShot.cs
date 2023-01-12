namespace Character.Combat.Skill
{
    public class AimShot : SkillObject
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
