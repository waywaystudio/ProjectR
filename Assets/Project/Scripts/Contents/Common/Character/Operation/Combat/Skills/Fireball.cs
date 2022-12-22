
using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class Fireball : BaseSkill
    {
        public override void CompleteSkill()
        {
            var hasDamageProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasStatusEffectProvider = TryGetComponent(out StatusEffectEntity statusEffectEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);

            if (hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    projectileEntity.Initialize(target);

                    if (hasDamageProvider)
                    {
                        projectileEntity.OnArrived.Register(damageEntity.GetInstanceID(), () => target.TakeDamage(damageEntity));
                    }

                    if (hasStatusEffectProvider)
                    {
                        projectileEntity.OnArrived.Register(statusEffectEntity.GetInstanceID(), () => target.TakeDeBuff(statusEffectEntity));
                    }
                });
            }

            base.CompleteSkill();
        }
    }
}
