using BehaviorDesigner.Runtime.Tasks;
using Common.Characters;
using Common.Characters.Behaviours;

namespace Common.Behavior.Characters
{
    [TaskCategory("Characters")]
    public class PositioningBehavior : Action
    {
        private CharacterBehaviour cb;
        private SkillTable sb;
        
        public override void OnStart()
        {
            cb = gameObject.GetComponentInParent<CharacterBehaviour>();
            
            TryGetComponent(out sb);
        }

        public override TaskStatus OnUpdate()
        {
            if (!sb.TryGetMostPrioritySkill(out var skill)) return TaskStatus.Failure;
            if (skill.MainTarget == null) return TaskStatus.Failure;

            var profitDistance = skill.PivotRange == 0.0f
                ? skill.AreaRange
                : skill.PivotRange;
            
            if (!PathfindingUtility.TryGetCombatPosition(skill.Cb,
                    skill.MainTarget,
                    profitDistance,
                    out var destination))
                return TaskStatus.Success;
            
            cb.Run(destination);
            
            return TaskStatus.Failure;
        }
    }
}
