using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using UnityEngine;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class MaxRangeMoveBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;
        private float range;
        private Vector2 threshold;
        private GameObject target;

        private Vector3 direction;
        private Vector3 targetPosition;
        private Vector3 characterPosition;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
            range = playerBehaviour.CharacterTargeting.AttackRange;
            threshold.x = 0.1f;
            threshold.y = 0.2f;
        }

        public override TaskStatus OnUpdate()
        {
            target = playerBehaviour.CharacterTargeting.FocusTarget;
            characterPosition = transform.position;
            targetPosition = target.transform.position;
            direction = (targetPosition - characterPosition).normalized;
            
            var rangeThreshold = range * Random.Range(threshold.x, threshold.y);
            var currentDistance = Vector3.Distance(targetPosition, characterPosition);
            var magnitude = Mathf.Abs(currentDistance - range) + rangeThreshold;
            var destination = characterPosition + direction * magnitude;

            // 사거리보다 너무 안쪽에 있다.
            if (currentDistance <= range * (1f - threshold.y))
            {
                direction *= -1f;
                magnitude = Mathf.Abs(currentDistance - range) - rangeThreshold;
                destination = characterPosition + direction * magnitude;
            }
            // 최대 사거리는 아니지만, 꽤 멀리있는 편이라면 그대로 있는다.
            else if (range * (1f - threshold.y) < currentDistance && currentDistance < range)
                return TaskStatus.Success;
            
            playerBehaviour.Run(destination);
            return TaskStatus.Success;
        }
    }
}
