namespace Common.Character.Operation.Combat.Skills
{
    public class Roar : BaseSkill
    {
        public override void StartSkill()
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
