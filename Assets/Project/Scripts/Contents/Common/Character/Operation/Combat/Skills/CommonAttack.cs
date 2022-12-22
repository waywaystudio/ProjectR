using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class CommonAttack : BaseSkill
    {
        public override void InvokeEvent()
        {
            var hasDamage = TryGetEntity(EntityType.Damage, out DamageEntity damageEntity);
            var hasTarget = TryGetEntity(EntityType.Target, out TargetEntity targetEntity);

            if (hasDamage && hasTarget)
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    target.TakeDamage(damageEntity);
                });
        }
    }
}