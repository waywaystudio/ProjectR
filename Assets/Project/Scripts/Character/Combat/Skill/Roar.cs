namespace Character.Combat.Skill
{
    public class Roar : BaseSkill
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (DamageEntity && StatusEffectEntity && TargetEntity)
            {
                TargetEntity.CombatTakerList.ForEach(target =>
                {
                    target.TakeDamage(DamageEntity);
                    target.TakeStatusEffect(StatusEffectEntity);
                });
            }
        }
        
    }
}
