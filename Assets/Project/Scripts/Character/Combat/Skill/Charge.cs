using Core;
using UnityEngine;

namespace Character.Combat.Skill
{
    public class Charge : BaseSkill
    {
        [SerializeField] private float dashSpeed = 30.0f;
        [SerializeField] private float offsetDistance = 3.5f;

        public override void InvokeEvent()
        {
            if (DamageEntity && TargetEntity)
                TargetEntity.Target.TakeDamage(DamageEntity);
                // TargetEntity.TakerList.ForEach(target => target.TakeDamage(DamageEntity));
        }

        // 위 처럼 구현하고 매 프레임에서 Target != null을 체크하면 안정성이 조금 올라간다.
        public override void ActiveSkill()
        {
            if (!TargetEntity) return;

            var takerTransform = TargetEntity.Target.Object.transform;
            var offset = Cb.Direction.Invoke() * (offsetDistance * -1f);
            var targetFrontPosition = takerTransform.position + offset;

            Cb.StatTable.Register(ActionCode, new MoveSpeedValue(dashSpeed));
            Cb.Run(targetFrontPosition, base.ActiveSkill);
        }

        protected override void CompleteSkill()
        {
            base.CompleteSkill();
            Cb.StatTable.Unregister(ActionCode, StatCode.MoveSpeed);
        }
    }
}
