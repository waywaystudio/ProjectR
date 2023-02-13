using BehaviorDesigner.Runtime.Tasks;
using Core;

namespace Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class IsAlive : Action
    {
        private ICombatProvider provider;
        
        public override void OnAwake()
        {
            provider = gameObject.GetComponentInParent<ICombatProvider>();
        }
        
        public override TaskStatus OnUpdate()
        {
            return provider.DynamicStatEntry.IsAlive.Value
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
