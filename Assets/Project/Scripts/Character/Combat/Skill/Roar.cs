namespace Character.Combat.Skill
{
    public class Roar : SkillObject
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (TargetModule)
            {
                if (DamageModule) TargetModule.TakeDamage(DamageModule);
                if (StatusEffectModule) TargetModule.TakeStatusEffect(StatusEffectModule);
            }
        }
        
    }
}
