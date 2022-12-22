using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class ChainHealing : BaseSkill
    {
        public override void CompleteSkill()
        {
            // var hasDamageProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasHealProvider = TryGetComponent(out HealEntity hasHealEntity);
            // var hasStatusEffectProvider = TryGetComponent(out StatusEffectEntity statusEffectEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);

            if (hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    projectileEntity.Initialize(target);

                    // if (hasDamageProvider)
                    // {
                    //     projectileEntity.OnArrived.Register(damageEntity.GetInstanceID(), () => target.TakeDamage(damageEntity));
                    // }
                    
                    if (hasHealProvider)
                    {
                        projectileEntity.OnArrived.Register(hasHealEntity.GetInstanceID(), () => target.TakeHeal(hasHealEntity));
                    }

                    // if (hasStatusEffectProvider)
                    // {
                    //     // projectileEntity.OnArrived.Register(projectileInstanceID, () => projectileEntity.Damage(damageEntity));
                    // }
                });
            }

            base.CompleteSkill();
        }
    }
}
