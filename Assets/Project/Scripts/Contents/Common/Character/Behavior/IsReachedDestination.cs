using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class IsReachedDestination : Action
    {
        private PlayerBehaviour playerBehaviour;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            return playerBehaviour.IsDestinationReached
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
