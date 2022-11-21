using System;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Common.Character
{
    [Serializable]
    public class CharacterMoveBehavior : Action
    {
        [LabelText("Seeker")]
        [SerializeField] private Seeker agent;
        private AIPath aiPath;

        public override void OnAwake()
        {
            agent = GetComponent<Seeker>();
            aiPath = GetComponent<AIPath>();
        }

        public override TaskStatus OnUpdate()
        {
            
            
            return TaskStatus.Success;
        }
    }
}
