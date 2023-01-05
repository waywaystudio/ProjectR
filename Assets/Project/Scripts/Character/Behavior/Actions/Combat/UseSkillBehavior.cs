using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;

namespace Character.Behavior.Actions.Combat
{
    [TaskCategory("Character/Combat")]
    public class UseSkillBehavior : Action
    {
        public SharedInt TargetSkillID;
        
        private CombatBehaviour combat;
        

        public override void OnAwake()
        {
            combat = GetComponent<CombatBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }

            // Remove When All Condition Check Function Implemented
            if (skill.IsSkillReady)
            {
                combat.UseSkill(skill);
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}
