using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Character.Behavior
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character")]
    public class CheckBoolean : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Is True Check Value")]
        public SharedBool Condition;

        public override TaskStatus OnUpdate() => Condition.Value
            ? TaskStatus.Success 
            : TaskStatus.Failure;
        
        public override void OnReset()
        {
            Condition = false;
        }
    }
}
