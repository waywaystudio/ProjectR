using BehaviorDesigner.Runtime.Tasks;

namespace Common.Character.Operation.Combating.Behavior
{
    [TaskCategory("Character/Combat")]
    public class CurrentSkillBehavior : Action
    {
        private Combat combat;
                
        public override void OnAwake()
        {
            combat = GetComponent<Combat>();
        }
        
        public override TaskStatus OnUpdate() => combat.IsCurrentSkillFinished
            ? TaskStatus.Success 
            : TaskStatus.Failure;
    }
}
