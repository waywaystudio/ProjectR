using Common.Character.Operation.Combating.Entity;

namespace Common.Character.Operation.Combating.Skills
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
                    target.TakeDeBuff(statusEffectEntity.Name, statusEffectEntity);
                });
            }
        }
        
    }
}
