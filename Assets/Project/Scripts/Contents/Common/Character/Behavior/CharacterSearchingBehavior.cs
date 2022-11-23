using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using Core;

namespace Common.Character
{
    public class CharacterSearchingBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            if (playerBehaviour.CharacterTargeting.SearchedTargets.IsNullOrEmpty())
            {
                return TaskStatus.Running;
            }

            return TaskStatus.Success;
        }
    }
}
