using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;

namespace Character.Behavior.Combat
{
    [TaskCategory("Character/Combat")]
    public class CurrentSkillBehavior : Action
    {
        private SkillBehaviour combat;
                
        public override void OnAwake()
        {
            combat = GetComponent<SkillBehaviour>();
        }
        
        public override TaskStatus OnUpdate() => combat.IsCurrentSkillFinished
            ? TaskStatus.Success 
            : TaskStatus.Failure;
    }
}
