using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Behavior
{
    [TaskCategory("Character/Combat")]
    public class PositioningBehavior : Action
    {
        private CombatOperation combat;
        private CharacterBehaviour cb;
        

        public override void OnAwake()
        {
            combat = GetComponent<CombatOperation>();
            cb = combat.GetComponentInParent<CharacterBehaviour>();
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

            var isMovable = combat.Positioning.TryGetCombatPosition(
                targetEntity.CombatTaker, targetEntity.Range, out var destination);

            if (!isMovable && cb.IsReached.Invoke())
            {
                return TaskStatus.Success;
            }
            
            cb.Run(destination); // destination
            return TaskStatus.Failure;
        }
    }
}
