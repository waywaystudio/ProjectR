using Core;
using UnityEngine;

namespace Character.Combat.Skill
{
    public class Charge : SkillObject
    {
        [SerializeField] private float dashSpeed = 30.0f;
        [SerializeField] private float offsetDistance = 3.5f;

        private CharacterBehaviour cb;
        private IPathfinding pathEngine;

        public override void Hit()
        {
            if (DamageModule && TargetModule)
            {
                TargetModule.TakeDamage(DamageModule);
            }
        }

        // 위 처럼 구현하고 매 프레임에서 Target != null을 체크하면 안정성이 조금 올라간다.
        public override void Active()
        {
            if (!TargetModule) return;

            var takerTransform = TargetModule.Target.Object.transform;
            var offset = pathEngine.Direction * (offsetDistance * -1f);
            var targetFrontPosition = takerTransform.position + offset;

            cb.StatTable.Register(ActionCode, new MoveSpeedValue(dashSpeed));
            cb.Run(targetFrontPosition, base.Active);
        }

        public override void Complete()
        {
            base.Complete();
            cb.StatTable.Unregister(ActionCode, StatCode.MoveSpeed);
        }


        protected override void Awake()
        {
            base.Awake();

            cb         = GetComponentInParent<CharacterBehaviour>();
            pathEngine = cb.PathfindingEngine;
        }
    }
}
