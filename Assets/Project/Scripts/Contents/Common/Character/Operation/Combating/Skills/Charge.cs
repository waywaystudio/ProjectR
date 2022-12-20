using Common.Character.Operation.Combating.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combating.Skills
{
    public class Charge : BaseSkill
    {
        [SerializeField] private float dashSpeed = 30.0f;
        [SerializeField] private float offsetDistance = 3.5f;
        
        public override void InvokeEvent()
        {
            var hasProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            
            if (hasProvider && hasTargetList)
                targetEntity.CombatTakerList.ForEach(target => target.TakeDamage(damageEntity));
        }

        // 위 처럼 구현하고 매 프레임에서 Target != null을 체크하면 안정성이 조금 올라간다.
        public override void ActiveSkill()
        {
            if (!TryGetComponent(out TargetEntity targetEntity)) return;

            var takerTransform = targetEntity.CombatTaker.Taker.transform;
            var offset = Cb.Direction.Invoke() * (offsetDistance * -1f);
            var targetFrontPosition = takerTransform.position + offset;
            
            Cb.MoveSpeedTable.RegisterSumType("Charge", () => dashSpeed, true);
            Cb.Run(targetFrontPosition, base.ActiveSkill);
        }

        public override void CompleteSkill()
        {
            base.CompleteSkill();
            Cb.MoveSpeedTable.UnregisterSumType("Charge");
        }
    }
}
