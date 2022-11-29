using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using UnityEngine;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class IsInRangedBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;
        private GameObject target;
        private float range;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            target = playerBehaviour.CharacterTargeting.FocusTarget;
            range = playerBehaviour.CharacterTargeting.AttackRange;
            
            var characterPosition = transform.position;
            var targetPosition = target.transform.position;
            var currentDistance = Vector3.Distance(targetPosition, characterPosition);
            var inRanged = currentDistance <= range;
            
            return inRanged ? TaskStatus.Success 
                            : TaskStatus.Failure;
        }
    }
}
