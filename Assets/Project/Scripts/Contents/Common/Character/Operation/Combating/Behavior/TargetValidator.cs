using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Operation.Combating.Entity;
using Core;

namespace Common.Character.Operation.Combating.Behavior
{
    [TaskCategory("Character/Combat")]
    public class TargetValidator : Action
    {
        private ICombatTaker taker;
        private Combat combat;
        
        public override void OnAwake()
        {
            combat = GetComponent<Combat>();
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
