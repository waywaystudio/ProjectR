using BehaviorDesigner.Runtime.Tasks;
using Common;

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
            return provider.Alive.Value
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
