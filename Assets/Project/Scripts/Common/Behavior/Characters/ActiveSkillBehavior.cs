using BehaviorDesigner.Runtime.Tasks;
using Common.Characters.Behaviours;

namespace Common.Behavior.Characters
{
    [TaskCategory("Characters")]
    public class ActiveSkillBehavior : Action
    {
        private SkillTable ab;
        
        public override void OnAwake()
        {
            TryGetComponent(out ab);
        }
        
        public override TaskStatus OnUpdate()
        {
            if (!ab.TryGetMostPrioritySkill(out var skill)) return TaskStatus.Failure;
            if (skill.MainTarget is null) return TaskStatus.Failure;

            ab.Active(skill.DataIndex, skill.MainTarget.gameObject.transform.position);
                
            return TaskStatus.Success;
        }
    }
}
