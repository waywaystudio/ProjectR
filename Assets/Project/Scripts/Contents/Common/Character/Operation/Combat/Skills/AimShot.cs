using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class AimShot : BaseSkill
    {
        public override void CompleteSkill()
        {
            // var hasDamage = TryGetComponent(out DamageEntity damageEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);

            if (hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    // 만약 여러 타겟이면 여기서 다 쏴줘야하는거 아녀?
                    // 마지막에 쏴주면 대상하나만 쏘는거 같은데;
                    projectileEntity.Fire(Sender, target);
                    // projectileEntity.OnArrived.Register(InstanceID, () => target.TakeDamage(damageEntity));
                });
            }

            base.CompleteSkill();
        }
    }
}
