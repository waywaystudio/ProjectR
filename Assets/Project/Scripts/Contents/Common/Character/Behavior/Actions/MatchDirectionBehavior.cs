using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using DG.Tweening;
using UnityEngine;

namespace Common.Character.Behavior
{
    [TaskCategory("Character")]
    public class MatchDirectionBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;
        private Vector3 targetPosition;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            targetPosition = playerBehaviour.FocusTarget.transform.position;
            playerBehaviour.transform.DOLookAt(targetPosition, 0.15f);
            
            return TaskStatus.Success;
        }
    }
}
