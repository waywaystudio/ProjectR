using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using UnityEngine;

namespace Common.Character
{
    public class CharacterIdleBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            playerBehaviour.Idle();

            return TaskStatus.Success;
        }
    }
}
