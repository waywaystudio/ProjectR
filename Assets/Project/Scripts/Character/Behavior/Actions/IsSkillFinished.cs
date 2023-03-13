using BehaviorDesigner.Runtime.Tasks;
using Character.Actions;
using Common.Actions;

namespace Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class IsSkillFinished : Action
    {
        private ActionBehaviour ab;
        
        public override void OnAwake()
        {
            TryGetComponent(out ab);
        }
        
        public override TaskStatus OnUpdate() => ab.IsSkillEnded
            ? TaskStatus.Success 
            : TaskStatus.Failure;
    }
}
