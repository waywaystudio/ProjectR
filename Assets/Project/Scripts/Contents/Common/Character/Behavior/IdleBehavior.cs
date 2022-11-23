using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class IdleBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            playerBehaviour.Idle();

            return TaskStatus.Success;
        }
    }
}
