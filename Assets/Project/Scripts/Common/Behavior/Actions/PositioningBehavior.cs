using BehaviorDesigner.Runtime.Tasks;
using Common.Characters;
using Common.Characters.Behaviours;
using Common.Systems;

namespace Character.Behavior.Actions
{
    [TaskCategory("Character/Combat")]
    public class PositioningBehavior : Action
    {
        private CharacterBehaviour cb;
        private SkillBehaviour ab;
        
        public override void OnAwake()
        {
            cb = gameObject.GetComponentInParent<CharacterBehaviour>();
            TryGetComponent(out ab);
        }

        public override TaskStatus OnUpdate()
        {
            if (!ab.TryGetMostPrioritySkill(out var skill)) return TaskStatus.Failure;
            if (skill.MainTarget == null) return TaskStatus.Failure;
            if (!PathfindingUtility.TryGetCombatPosition(skill.Cb,
                    skill.MainTarget,
                    skill.Range,
                    out var destination))
                return TaskStatus.Success;
            
            cb.Run(destination);
            
            return TaskStatus.Failure;
        }
    }
}
