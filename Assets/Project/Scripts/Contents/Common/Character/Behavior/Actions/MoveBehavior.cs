using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class MoveBehavior : Action
    {
        private OLD_CharacterBehaviour oldCharacterBehaviour;

        public Vector3 Destination => oldCharacterBehaviour.Destination;
        public bool IsFinished => oldCharacterBehaviour.IsDestinationReached;

        public override void OnAwake()
        {
            oldCharacterBehaviour = GetComponent<OLD_CharacterBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
