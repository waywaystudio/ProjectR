namespace Character.Combat.Skill
{
    public class Fireball : SkillObject
    {
        protected override void CompleteSkill()
        {
            if (TargetModule && ProjectileModule)
            {
                TargetModule.TakeProjectile(ProjectileModule);
            }

            base.CompleteSkill();
        }
    }
}
