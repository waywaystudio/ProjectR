using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class IsReachedDestination : Action
    {
        private PlayerBehaviour pb;

        public override void OnAwake()
        {
            pb = GetComponent<PlayerBehaviour>();
        }
        
        public override TaskStatus OnUpdate() 
            => pb.IsDestinationReached 
                ? TaskStatus.Success 
                : TaskStatus.Failure;
        
    }
}
