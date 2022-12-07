namespace Common.Character.Skills
{
    using Core;
    using Entity;

    public class AimShot : SkillAttribution
    {
        public override void InvokeEvent()
        {
            var hasProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);

            if (hasProvider && hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    projectileEntity.Initialize(damageEntity, target);
                    target.TakeDamage(damageEntity);
                });
            }
        }
    }
}
