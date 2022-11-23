using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class UntilMoveFinished : Action
    {
        private PlayerBehaviour playerBehaviour;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            return playerBehaviour.IsFinished ? TaskStatus.Success 
                                              : TaskStatus.Running;
        }
    }
}
