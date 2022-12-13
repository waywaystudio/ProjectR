using Common.Character.Operation.Combating.Entity;

namespace Common.Character.Operation.Combating.Skills
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
