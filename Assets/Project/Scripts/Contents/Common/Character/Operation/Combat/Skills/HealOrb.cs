using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class HealOrb : BaseSkill
    {
        public override void StartSkill()
        {
            base.StartSkill();
            
            var hasHealProvider = TryGetComponent(out HealEntity hasHealEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);

            if (hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    projectileEntity.Initialize(target);

                    if (hasHealProvider)
                    {
                        // TODO. 이렇게 짜면 충돌한 대상에게 힐이 들어가는게 아니라, 조준한 대상에게 힐이 들어간다.
                        projectileEntity.OnCollided.Register(hasHealEntity.GetInstanceID(),() => target.TakeHeal(hasHealEntity));
                    }
                });
            }
        }
    }
}
