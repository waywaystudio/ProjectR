using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class Fireball : BaseSkill
    {
        public override void CompleteSkill()
        {
            // var hasDamage = TryGetComponent(out DamageEntity damageEntity);
            // var hasStatusEffectProvider = TryGetComponent(out StatusEffectEntity statusEffectEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);

            if (hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    projectileEntity.Fire(Sender, target);

                    // if (hasDamage)
                    // {
                    //     projectileEntity.OnArrived.Register
                    //         (damageEntity.InstanceID, () => target.TakeDamage(damageEntity));
                    // }
                    //
                    // if (hasStatusEffectProvider)
                    // {
                    //     projectileEntity.OnArrived.Register
                    //         (statusEffectEntity.InstanceID, () => target.TakeStatusEffect(statusEffectEntity.Sender));
                    // }
                });
            }

            base.CompleteSkill();
        }
    }
}
