using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character")]
    public class CheckBoolean : Action
    {
        [Tooltip("Is True Check Value")]
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
