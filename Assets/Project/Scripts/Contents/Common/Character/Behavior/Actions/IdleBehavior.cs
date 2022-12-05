using BehaviorDesigner.Runtime.Tasks;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class IdleBehavior : Action
    {
        private OLD_CharacterBehaviour oldCharacterBehaviour;

        public override void OnAwake()
        {
            oldCharacterBehaviour = GetComponent<OLD_CharacterBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            oldCharacterBehaviour.Idle();

            return TaskStatus.Success;
        }
    }
}
