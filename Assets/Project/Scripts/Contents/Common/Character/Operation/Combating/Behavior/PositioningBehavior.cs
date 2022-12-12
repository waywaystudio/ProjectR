using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Operation.Combating.Entity;

namespace Common.Character.Operation.Combating.Behavior
{
    [TaskCategory("Character/Combat")]
    public class PositioningBehavior : Action
    {
        private Combat combat;
        private CharacterBehaviour cb;
        

        public override void OnAwake()
        {
            combat = GetComponent<Combat>();
            cb = combat.Cb;
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

            var isMovable = combat.CombatPosition.TryGetCombatPosition(
                targetEntity.CombatTaker, targetEntity.Range, out var destination);

            if (!isMovable && cb.IsReached.Invoke()) return TaskStatus.Success;
            
            cb.Run(destination); // destination
            return TaskStatus.Failure;
        }
    }
}
