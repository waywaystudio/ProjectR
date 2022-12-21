using BehaviorDesigner.Runtime.Tasks;

namespace Common.Character.Operation.Combat.Behavior
{
    [TaskCategory("Character/Combat")]
    public class CurrentSkillBehavior : Action
    {
        private Combat.Combating combat;
                
        public override void OnAwake()
        {
            combat = GetComponent<Combat.Combating>();
        }
        
        public override TaskStatus OnUpdate() => combat.IsCurrentSkillFinished
            ? TaskStatus.Success 
            : TaskStatus.Failure;
    }
}
