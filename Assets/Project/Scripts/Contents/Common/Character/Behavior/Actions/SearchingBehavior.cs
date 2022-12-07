using BehaviorDesigner.Runtime.Tasks;
using Core;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class SearchingBehavior : Action
    {
        private OLD_CharacterBehaviour pb;

        public override void OnAwake()
        {
            pb = GetComponent<OLD_CharacterBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            // return pb.FocusTarget.IsNullOrEmpty() 
            //     ? TaskStatus.Failure
            //     : TaskStatus.Success;

            return TaskStatus.Failure;
        }
    }
}
