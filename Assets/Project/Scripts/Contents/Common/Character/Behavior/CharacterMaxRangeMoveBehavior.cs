using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using UnityEngine;

namespace Common.Character.Behavior
{
    public class CharacterMaxRangeMoveBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;
        private float range;
        private Vector2 threshold;
        private GameObject target;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
            range = 5f;
            threshold.x = 0.1f;
            threshold.y = 0.15f;
        }

        public override TaskStatus OnUpdate()
        {
            target = playerBehaviour.CharacterTargeting.FocusTarget;
            
            if (target == null) return TaskStatus.Running;
            
            var characterPosition = transform.position;
            var targetPosition = target.transform.position;
            var direction = (targetPosition - characterPosition).normalized;
            var currentDistance = Vector3.Distance(targetPosition, characterPosition);
            var magnitude = currentDistance - range * (1 + Random.Range(threshold.x, threshold.y));
            var destination = direction * magnitude;
            
            playerBehaviour.Run(destination);
            
            return TaskStatus.Success;
        }
    }
}
