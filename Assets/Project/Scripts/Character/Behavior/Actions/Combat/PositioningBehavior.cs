using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;

namespace Character.Behavior.Actions.Combat
{
    [TaskCategory("Character/Combat")]
    public class PositioningBehavior : Action
    {
        private CombatBehaviour combat;
        private CharacterBehaviour cb;
        

        public override void OnAwake()
        {
            combat = GetComponent<CombatBehaviour>();
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
