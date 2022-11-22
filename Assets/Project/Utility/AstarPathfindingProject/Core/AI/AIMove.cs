using System;
using UnityEngine;

namespace Pathfinding
{
    using RVO;
    
    /// <summary>
    /// Inherit AIPath Class for Custom Hierarchy by Override FindComponents. 
    /// </summary>
    public class AIMove : AIPath
    {
        [SerializeField] private Transform rootObject;

        private Action onTargetReached;
        
        public Transform RootObject => rootObject;

        public void Initialize(Action onTargetReached)
        {
            this.onTargetReached = onTargetReached;
        }

        public override void OnTargetReached()
        {
            onTargetReached?.Invoke();
        }

        public override void FindComponents()
        {
            tr = rootObject;
            rigid = rootObject.GetComponent<Rigidbody>();
            seeker = GetComponent<Seeker>();
            rvoController = GetComponent<RVOController>();
            controller = GetComponent<CharacterController>();
            rigid2D = GetComponent<Rigidbody2D>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onTargetReached = null;
        }
    }
}


