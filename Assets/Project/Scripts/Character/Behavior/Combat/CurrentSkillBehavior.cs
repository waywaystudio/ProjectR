using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;

namespace Character.Behavior.Combat
{
    [TaskCategory("Character/Combat")]
    public class CurrentSkillBehavior : Action
    {
        private OldSkillBehaviour combat;
                
        public override void OnAwake()
        {
            combat = GetComponent<OldSkillBehaviour>();
        }
        
        public override TaskStatus OnUpdate() => combat.IsCurrentSkillFinished
            ? TaskStatus.Success 
            : TaskStatus.Failure;
    }
}
