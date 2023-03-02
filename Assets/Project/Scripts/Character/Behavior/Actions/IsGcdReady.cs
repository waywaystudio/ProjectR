using BehaviorDesigner.Runtime.Tasks;
using Character.Actions;

namespace Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class IsGcdReady : Action
    {
        private CharacterAction ab;
        
        public override void OnAwake()
        {
            TryGetComponent(out ab);
        }
        
        public override TaskStatus OnUpdate() => ab.GlobalConditions.IsAllTrue
            ? TaskStatus.Success 
            : TaskStatus.Failure;
    }
}
