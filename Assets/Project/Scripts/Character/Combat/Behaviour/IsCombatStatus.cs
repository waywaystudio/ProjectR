using BehaviorDesigner.Runtime.Tasks;
using Core;

namespace Character.Combat.Behaviour
{
    public class IsCombatStatus : Action
    {
        private CharacterBehaviour cb;
        
        public override void OnAwake()
        {
            cb = gameObject.GetComponentInParent<CharacterBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            return cb.BehaviourStatus == BehaviourStatus.Combat
                    ? TaskStatus.Success
                    : TaskStatus.Failure;
        }
    }
}
