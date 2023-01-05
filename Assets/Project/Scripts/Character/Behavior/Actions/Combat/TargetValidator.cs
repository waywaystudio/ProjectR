using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;
using Core;

namespace Character.Behavior.Actions.Combat
{
    [TaskCategory("Character/Combat")]
    public class TargetValidator : Action
    {
        private ICombatTaker taker;
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

            var targetEntity = skill.TargetEntity;

            if (targetEntity is null)
            {
                return TaskStatus.Success;
            }

            taker = targetEntity.CombatTaker;

            var isValid =
                taker != null &&
                taker.DynamicStatEntry.IsAlive.Value;
                // Add more condition...

            return isValid
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
