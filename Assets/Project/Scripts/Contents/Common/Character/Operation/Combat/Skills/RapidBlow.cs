using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class RapidBlow : BaseSkill
    {
        public override void InvokeEvent()
        {
            var hasProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            
            if (hasProvider && hasTargetList)
                targetEntity.CombatTakerList.ForEach(target => target.TakeDamage(damageEntity));
        }
    }
}
