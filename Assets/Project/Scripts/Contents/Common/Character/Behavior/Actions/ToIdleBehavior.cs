using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Operation.Combat;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class ToIdleBehavior : Action
    {
        private CharacterBehaviour cb;

        public override void OnAwake()
        {
            TryGetComponent<Combating>(out var combat);

            cb = combat.GetComponentInParent<CharacterBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            cb.Idle();

            return TaskStatus.Success;
        }
    }
}
