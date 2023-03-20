using BehaviorDesigner.Runtime.Tasks;
using Common.Skills;

namespace Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class IsGcdReady : Action
    {
        private CoolTimer coolTimer;
        
        public override void OnAwake()
        {
            TryGetComponent(out coolTimer);
        }
        
        public override TaskStatus OnUpdate() => coolTimer.RemainCoolTime.Value != 0f
            ? TaskStatus.Failure
            : TaskStatus.Success;
    }
}
