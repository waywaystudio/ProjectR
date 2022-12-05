using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class IsInRangedBehavior : Action
    {
        private OLD_CharacterBehaviour oldCharacterBehaviour;
        private GameObject target;
        private float range;

        public override void OnAwake()
        {
            oldCharacterBehaviour = GetComponent<OLD_CharacterBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            target = oldCharacterBehaviour.Targeting.FocusTarget;
            range = oldCharacterBehaviour.Targeting.AttackRange;
            
            var characterPosition = transform.position;
            var targetPosition = target.transform.position;
            var currentDistance = Vector3.Distance(targetPosition, characterPosition);
            var inRanged = currentDistance <= range;
            
            return inRanged ? TaskStatus.Success 
                            : TaskStatus.Failure;
        }
    }
}
