using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using Core;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class SearchingBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            return playerBehaviour.CharacterTargeting.SearchedTargets.IsNullOrEmpty() ? TaskStatus.Failure 
                                                                                      : TaskStatus.Success;
        }
    }
}
