using BehaviorDesigner.Runtime.Tasks;

namespace Common.Behavior.Characters
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Characters")]
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
