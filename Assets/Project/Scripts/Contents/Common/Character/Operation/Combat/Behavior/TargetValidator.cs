using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Operation.Combat.Entity;
using Core;

namespace Common.Character.Operation.Combat.Behavior
{
    [TaskCategory("Character/Combat")]
    public class TargetValidator : Action
    {
        private ICombatTaker taker;
        private Combat.Combating combat;
        
        public override void OnAwake()
        {
            combat = GetComponent<Combat.Combating>();
        }
        
        public override TaskStatus OnUpdate()
        {
            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }

            if (!skill.TryGetEntity<TargetEntity>(EntityType.Target, out var targetEntity))
            {
                return TaskStatus.Success;
            }

            taker = targetEntity.CombatTaker;

            var isValid =
                taker != null &&
                taker.IsAlive;
                // Add more condition...

            return isValid
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
