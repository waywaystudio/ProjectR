using BehaviorDesigner.Runtime.Tasks;
using Common.Characters.Behaviours;

namespace Character.Behavior.Actions
{
    [TaskCategory("Character/Combat")]
    public class ActiveSkillBehavior : Action
    {
        private SkillBehaviour ab;
        
        public override void OnAwake()
        {
            TryGetComponent(out ab);
        }
        
        public override TaskStatus OnUpdate()
        {
            if (!ab.TryGetMostPrioritySkill(out var skill)) return TaskStatus.Failure;
            if (skill.MainTarget is null) return TaskStatus.Failure;

            ab.Active(skill, skill.MainTarget.Object.transform.position);
                
            return TaskStatus.Success;
        }
    }
}
