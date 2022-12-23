using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class Roar : BaseSkill
    {
        public override void StartSkill()
        {
            base.StartSkill();
            
            var hasDamage = TryGetComponent(out DamageEntity damageEntity);
            var hasStatusEffect = TryGetComponent(out StatusEffectEntity statusEffectEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);

            if (hasDamage && hasStatusEffect && hasTargetList)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    target.TakeDamage(damageEntity);
                    target.TakeStatusEffect(statusEffectEntity.Sender);
                });
            }
        }
        
    }
}
