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
        
        /// <summary>
        /// CallBack When Completely Reached.
        /// 도착 전에 목적지를 바꾸거나, 다른 행동을 하면 호출되지 않음.
        /// </summary>
        public override void OnTargetReached()
        {
            Callback?.Invoke();
        }
    }
}


