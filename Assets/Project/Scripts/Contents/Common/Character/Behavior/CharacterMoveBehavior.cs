using System;
using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Common.Character
{
    public class CharacterMoveBehavior : Action
    {
        private PlayerBehaviour playerBehaviour;

        public Vector3 Destination => playerBehaviour.Destination;
        public bool IsFinished => playerBehaviour.IsFinished;

        public override void OnAwake()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
