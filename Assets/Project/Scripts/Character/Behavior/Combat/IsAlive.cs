using BehaviorDesigner.Runtime.Tasks;

namespace Character.Behavior.Combat
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class IsAlive : Action
    {
        private CharacterBehaviour cb;
        
        public override void OnAwake()
        {
            cb = gameObject.GetComponentInParent<CharacterBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            return cb.DynamicStatEntry.IsAlive.Value
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
