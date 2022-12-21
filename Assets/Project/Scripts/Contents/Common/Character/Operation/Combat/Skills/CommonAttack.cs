using Common.Character.Operation.Combat.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combat.Skills
{
    public class CommonAttack : BaseSkill
    {
        public override void InvokeEvent()
        {
            var hasProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            
            if (hasProvider && hasTargetList)
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    target.TakeDamage(damageEntity);
                });
        }
    }
}