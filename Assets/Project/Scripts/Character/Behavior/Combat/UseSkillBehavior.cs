using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;

namespace Character.Behavior.Combat
{
    [TaskCategory("Character/Combat")]
    public class UseSkillBehavior : Action
    {
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
