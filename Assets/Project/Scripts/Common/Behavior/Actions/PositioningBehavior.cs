using BehaviorDesigner.Runtime.Tasks;
using Common.Actions;
using Common.Systems;

namespace Character.Behavior.Actions
{
    [TaskCategory("Character/Combat")]
    public class PositioningBehavior : Action
    {
        private ActionBehaviour ab;
        
        public override void OnAwake()
        {
            TryGetComponent(out ab);
        }

        public override TaskStatus OnUpdate()
        {
            if (!ab.TryGetMostPrioritySkill(out var skill)) return TaskStatus.Failure;
            if (skill.MainTarget == null) return TaskStatus.Failure;
            if (!PathfindingUtility.TryGetCombatPosition(skill.Provider,
                    skill.MainTarget,
                    skill.Range,
                    out var destination))
                return TaskStatus.Success;
            
            ab.Run(destination);
            
            return TaskStatus.Failure;
        }
    }
}
