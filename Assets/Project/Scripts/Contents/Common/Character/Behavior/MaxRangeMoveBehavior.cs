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
        private Vector3 destination;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
            range = 6f;
            threshold.x = 0.1f;
            threshold.y = 0.2f;
        }

        public override TaskStatus OnUpdate()
        {
            target = playerBehaviour.CharacterTargeting.FocusTarget;
            
            if (target == null) 
                return TaskStatus.Running;
            
            var characterPosition = transform.position;
            var targetPosition = target.transform.position;
            var currentDistance = Vector3.Distance(targetPosition, characterPosition);
            var magnitude = Mathf.Abs(currentDistance - range);

            // 멀리 있는 경우 접근하기
            // ex. Current 100, Range 6f
            if (currentDistance > range)
            {
                direction = (targetPosition - characterPosition).normalized;
                destination = characterPosition + direction * (magnitude * (1f + Random.Range(threshold.x, threshold.y)));
            }
            // 이미 범위 안에 있는 경우, threshold를 바탕으로 뒤로 밀거나 유지
            else
            {
                // threshold 범위조차 벗어난 경우, 뒤로간다.
                // ex. Current 4, Range 6f
                if (currentDistance <= range * (1f - threshold.y))
                {
                    direction = (characterPosition - targetPosition).normalized;
                    destination = characterPosition + direction * magnitude;
                }
                else
                {
                    return TaskStatus.Success;
                }
            }
            
            playerBehaviour.Run(destination);
            return TaskStatus.Success;
        }
    }
}
