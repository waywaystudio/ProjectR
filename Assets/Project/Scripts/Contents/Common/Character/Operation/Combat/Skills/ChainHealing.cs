using Common.Character.Operation.Combat.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combat.Skills
{
    public class ChainHealing : BaseSkill
    {
        public override void CompleteSkill()
        {
            var hasHeal = TryGetComponent(out HealEntity healEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);

            if (hasHeal && hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    projectileEntity.Fire(healEntity, target);
                    // projectileEntity.OnArrived.Register(InstanceID, () => target.TakeHeal(healEntity));
                });
            }

            base.CompleteSkill();
        }
    }
}
