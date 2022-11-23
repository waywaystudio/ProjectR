using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class MoveBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;

        public Vector3 Destination => playerBehaviour.Destination;
        public bool IsFinished => playerBehaviour.IsFinished;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
