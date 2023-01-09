namespace Character.Combat.Skill
{
    public class Roar : BaseSkill
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (DamageEntity)
            {
                if (DamageEntity) TargetEntity.Target.TakeDamage(DamageEntity);
                if (StatusEffectEntity) StatusEffectEntity.Effecting(TargetEntity.Target);
            }
        }
        
    }
}
