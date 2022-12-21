using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class CommonAttack : BaseSkill
    {
        public override void InvokeEvent()
        {
            var hasProvider = TryGetEntity(EntityType.Damage, out DamageEntity damageEntity);
            var hasTargetList = TryGetEntity(EntityType.Target, out TargetEntity targetEntity);

            if (hasProvider && hasTargetList)
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    target.TakeDamage(damageEntity);
                });
        }
    }
}