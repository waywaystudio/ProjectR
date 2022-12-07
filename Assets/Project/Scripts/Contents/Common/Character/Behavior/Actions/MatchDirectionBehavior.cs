using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class MatchDirectionBehavior : Action
    {
        private OLD_CharacterBehaviour oldCharacterBehaviour;
        private Vector3 targetPosition;

        public override void OnAwake()
        {
            oldCharacterBehaviour = GetComponent<OLD_CharacterBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            // targetPosition = oldCharacterBehaviour.FocusTarget.transform.position;
            oldCharacterBehaviour.transform.DOLookAt(targetPosition, 0.15f);
            
            return TaskStatus.Success;
        }
    }
}
