using Common.Character.Operation.Combating.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combating.Skills
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
                    Debug.Log("CommonAttack In");
                    target.TakeDamage(damageEntity);
                });
        }
    }
}