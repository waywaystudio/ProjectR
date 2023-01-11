namespace Character.Combat.Skill
{
    public class Fireball : SkillObject
    {
        protected override void CompleteSkill()
        {
            if (TargetModule && ProjectileModule)
                ProjectileModule.Fire(TargetModule.Target);

            base.CompleteSkill();
        }
    }
}
