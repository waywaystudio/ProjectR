using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class IsAbleToAttack : Action
    {
        private PlayerBehaviour playerBehaviour;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            var result = playerBehaviour.HasPath ? playerBehaviour.IsFinished 
                                                     : playerBehaviour.IsInRange;

            return result ? TaskStatus.Success 
                          : TaskStatus.Failure;
        }
    }
}
