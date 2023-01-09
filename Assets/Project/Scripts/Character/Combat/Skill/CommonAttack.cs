namespace Character.Combat.Skill
{
    public class CommonAttack : BaseSkill
    {
        public override void InvokeEvent()
        {
            // TargetEntity.Target.TakeDamage(DamageEntity)
            // TargetEntity.TakeDamage(DamageEntity);
            // TargetEntity.TakeSpell(DamageEntity);
            // TargetEntity.TakeStatusEffect(StatusEffectEntity);
            
            if (DamageEntity && TargetEntity)
                TargetEntity.Target.TakeDamage(DamageEntity);
        }
    }
}