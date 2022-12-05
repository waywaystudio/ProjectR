using BehaviorDesigner.Runtime.Tasks;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class IsAbleToAttack : Action
    {
        private OLD_CharacterBehaviour oldCharacterBehaviour;

        public override void OnAwake()
        {
            oldCharacterBehaviour = GetComponent<OLD_CharacterBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            var result = oldCharacterBehaviour.HasPath ? oldCharacterBehaviour.IsDestinationReached 
                                                     : oldCharacterBehaviour.IsInRange;

            return result ? TaskStatus.Success 
                          : TaskStatus.Failure;
        }
    }
}
