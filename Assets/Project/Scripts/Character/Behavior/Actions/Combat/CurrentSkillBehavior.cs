using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;

namespace Character.Behavior.Actions.Combat
{
    [TaskCategory("Character/Combat")]
    public class CurrentSkillBehavior : Action
    {
        private CombatBehaviour combat;
                
        public override void OnAwake()
        {
            combat = GetComponent<CombatBehaviour>();
        }
        
        public override TaskStatus OnUpdate() => combat.IsCurrentSkillFinished
            ? TaskStatus.Success 
            : TaskStatus.Failure;
    }
}
