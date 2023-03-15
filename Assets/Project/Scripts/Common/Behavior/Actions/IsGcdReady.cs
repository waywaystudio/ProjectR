using BehaviorDesigner.Runtime.Tasks;
using Common.Characters.Behaviours;

namespace Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class IsGcdReady : Action
    {
        private SkillBehaviour ab;
        
        public override void OnAwake()
        {
            TryGetComponent(out ab);
        }
        
        public override TaskStatus OnUpdate() => ab.IsGlobalCooling
            ? TaskStatus.Failure
            : TaskStatus.Success;
    }
}
