using BehaviorDesigner.Runtime.Tasks;
using Character.Actions;
using Character.Skill;

namespace Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class IsGcdReady : Action
    {
        private ActionBehaviour ab;
        
        public override void OnAwake()
        {
            TryGetComponent(out ab);
        }
        
        public override TaskStatus OnUpdate() => ab.GlobalConditions.IsAllTrue
            ? TaskStatus.Success 
            : TaskStatus.Failure;
    }
}
