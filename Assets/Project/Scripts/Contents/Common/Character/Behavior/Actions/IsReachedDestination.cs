using BehaviorDesigner.Runtime.Tasks;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class IsReachedDestination : Action
    {
        private OLD_CharacterBehaviour pb;

        public override void OnAwake()
        {
            pb = GetComponent<OLD_CharacterBehaviour>();
        }
        
        public override TaskStatus OnUpdate() 
            => pb.IsDestinationReached 
                ? TaskStatus.Success 
                : TaskStatus.Failure;
        
    }
}
