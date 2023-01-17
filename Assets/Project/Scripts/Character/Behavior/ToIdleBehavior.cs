using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;

namespace Character.Behavior
{
    [TaskCategory("Character")]
    public class ToIdleBehavior : Action
    {
        private CharacterBehaviour cb;

        public override void OnAwake()
        {
            TryGetComponent<SkillBehaviour>(out var combat);

            cb = combat.GetComponentInParent<CharacterBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            cb.Idle();

            return TaskStatus.Success;
        }
    }
}
