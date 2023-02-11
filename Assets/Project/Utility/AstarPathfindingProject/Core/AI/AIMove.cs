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

        // Invoked When actual Arrived.
        public Action Callback { get; set; }

        public override void FindComponents()
        {
            tr = rootObject;
            rigid = rootObject.GetComponent<Rigidbody>();
            seeker = GetComponent<Seeker>();
            rvoController = GetComponent<RVOController>();
            controller = GetComponent<CharacterController>();
            rigid2D = GetComponent<Rigidbody2D>();
        }
        
        public override void OnTargetReached()
        {
            Callback?.Invoke();
        }
    }
}


