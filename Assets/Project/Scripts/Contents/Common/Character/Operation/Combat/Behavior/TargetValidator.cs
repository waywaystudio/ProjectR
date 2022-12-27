using BehaviorDesigner.Runtime.Tasks;

namespace Common.Character.Operation.Combat.Behavior
{
    [TaskCategory("Character/Combat")]
    public class TargetValidator : Action
    {
        private ICombatTaker taker;
        private Combating combat;
        
        public override void OnAwake()
        {
            combat = GetComponent<Combating>();
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
                taker.Status.IsAlive;
                // Add more condition...

            return isValid
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
