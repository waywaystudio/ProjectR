using BehaviorDesigner.Runtime.Tasks;
using Character.Actions;

namespace Character.Behavior.Actions
{
    [TaskCategory("Character/Combat")]
    public class ActiveSkillBehavior : Action
    {
        private CharacterAction ab;
        
        public override void OnAwake()
        {
            TryGetComponent(out ab);
        }
        
        public override TaskStatus OnUpdate()
        {
            if (!ab.TryGetMostPrioritySkill(out var skill)) return TaskStatus.Failure;
            if (skill.MainTarget is null) return TaskStatus.Failure;

            ab.ActiveSkill(skill, skill.MainTarget.Object.transform.position);
                
            return TaskStatus.Success;
        }
    }
}
