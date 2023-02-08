using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;
using Core;

namespace Character.Behavior.Combat
{
    [TaskCategory("Character/Combat")]
    public class PositioningBehavior : Action
    {
        private OldSkillBehaviour combat;
        private CharacterBehaviour cb;
        private IPathfinding pathfindingEngine;
        private ICombatTaker taker;

        public override void OnAwake()
        {
            combat            = GetComponent<OldSkillBehaviour>();
            cb                = combat.GetComponentInParent<CharacterBehaviour>();
            pathfindingEngine = cb.PathfindingEngine;
        }

        public override TaskStatus OnUpdate()
        {
            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }

            var targetModule = skill.TargetModule;

            if (targetModule is null)
            {
                return TaskStatus.Success;
            }

            taker = targetModule.Target;
            
            var isValid = taker != null && taker.DynamicStatEntry.IsAlive.Value;

            if (!isValid)
            {
                return TaskStatus.Failure;
            }

            var isMovable = PathfindingUtility.TryGetCombatPosition(cb, taker, targetModule.Range, out var destination);

            if (!isMovable && pathfindingEngine.IsReached)
            {
                return TaskStatus.Success;
            }
            
            cb.Run(destination, cb.Idle); // destination
            return TaskStatus.Failure;
        }
    }
}
