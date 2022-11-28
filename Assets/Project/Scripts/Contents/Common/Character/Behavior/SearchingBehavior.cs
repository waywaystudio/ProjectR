using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using Core;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class SearchingBehavior : Action
    {
        private PlayerBehaviour pb;

        public override void OnAwake()
        {
            pb = GetComponent<PlayerBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            return pb.FocusTarget.IsNullOrEmpty() 
                ? TaskStatus.Failure
                : TaskStatus.Success;
        }
    }
}
