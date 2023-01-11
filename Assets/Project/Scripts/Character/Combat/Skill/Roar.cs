namespace Character.Combat.Skill
{
    public class Roar : SkillObject
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (DamageModule)
            {
                if (DamageModule) TargetModule.Target.TakeDamage(DamageModule);
                if (StatusEffectModule) StatusEffectModule.Effecting(TargetModule.Target);
            }
        }
        
    }
}
