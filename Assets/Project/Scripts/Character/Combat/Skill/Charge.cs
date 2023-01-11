using Core;
using UnityEngine;

namespace Character.Combat.Skill
{
    public class Charge : SkillObject
    {
        [SerializeField] private float dashSpeed = 30.0f;
        [SerializeField] private float offsetDistance = 3.5f;

        private IPathfinding pathfindingEngine;

        public override void InvokeEvent()
        {
            if (DamageModule && TargetModule)
                TargetModule.Target.TakeDamage(DamageModule);
        }

        // 위 처럼 구현하고 매 프레임에서 Target != null을 체크하면 안정성이 조금 올라간다.
        public override void ActiveSkill()
        {
            if (!TargetModule) return;

            var takerTransform = TargetModule.Target.Object.transform;
            var offset = pathfindingEngine.Direction * (offsetDistance * -1f);
            var targetFrontPosition = takerTransform.position + offset;

            Cb.StatTable.Register(ActionCode, new MoveSpeedValue(dashSpeed));
            Cb.Run(targetFrontPosition, base.ActiveSkill);
        }

        protected override void CompleteSkill()
        {
            base.CompleteSkill();
            Cb.StatTable.Unregister(ActionCode, StatCode.MoveSpeed);
        }


        protected override void Awake()
        {
            base.Awake();

            pathfindingEngine = Cb.PathfindingEngine;
        }
    }
}
