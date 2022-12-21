using Common.Character.Operation.Combat.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combat.Skills
{
    public class BackStep : BaseSkill
    {
        /// <summary>
        /// How far from target behind.
        /// 4.2f almost same as CommonAttack.
        /// </summary>
        [SerializeField] private float backMagnitude = 4.2f;
        
        public override void InvokeEvent()
        {
            var hasProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            
            if (hasProvider && hasTargetList)
                targetEntity.CombatTakerList.ForEach(target => target.TakeDamage(damageEntity));
        }
        
        public override void StartSkill()
        {
            if (!TryGetComponent(out TargetEntity targetEntity)) return;
            
            var enemyTransform = targetEntity.CombatTaker.Object.transform;
            var enemyBehindPosition = enemyTransform.position + enemyTransform.forward * -backMagnitude;

            Cb.Teleport(enemyBehindPosition);
            base.StartSkill();
        }
    }
}
