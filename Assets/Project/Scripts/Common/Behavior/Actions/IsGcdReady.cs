using BehaviorDesigner.Runtime.Tasks;
using Common.Actions;

namespace Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class IsGcdReady : Action
    {
        private OldActionBehaviour ab;
        
        public override void OnAwake()
        {
            TryGetComponent(out ab);
        }
        
        public override TaskStatus OnUpdate() => ab.IsGlobalCooling
            ? TaskStatus.Failure
            : TaskStatus.Success;
    }
}
