namespace Character.Combat.Skill
{
    public class Roar : BaseSkill
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (DamageEntity && StatusEffectEntity && TargetEntity)
            {
                TargetEntity.Target.TakeDamage(DamageEntity);
                TargetEntity.Target.TakeStatusEffect(StatusEffectEntity);
            }
        }
        
    }
}
