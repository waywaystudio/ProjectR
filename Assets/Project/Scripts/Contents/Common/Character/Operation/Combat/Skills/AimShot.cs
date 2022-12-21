using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
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
