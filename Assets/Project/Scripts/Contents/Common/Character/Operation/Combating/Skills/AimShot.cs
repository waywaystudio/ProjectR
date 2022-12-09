using Common.Character.Operation.Combating.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combating.Skills
{
    public class AimShot : BaseSkill
    {
        public override void CompleteSkill()
        {
            var hasProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);
            
            if (hasProvider && hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    projectileEntity.Initialize(damageEntity, target);
                });
            }
            
            // Set Projectile Information before projectile Fire
            // base.CompleteSkill == Projectile Fire
            base.CompleteSkill();
        }
    }
}
