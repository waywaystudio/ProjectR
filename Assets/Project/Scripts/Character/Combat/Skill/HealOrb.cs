namespace Character.Combat.Skill
{
    public class HealOrb : SkillObject
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (ProjectileModule && TargetModule)
            {
                TargetModule.TakeProjectile(ProjectileModule);
            }
        }
    }
}
