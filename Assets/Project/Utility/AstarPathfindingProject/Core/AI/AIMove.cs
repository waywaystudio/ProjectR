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
        
        public Transform RootObject => rootObject;

        public override void FindComponents()
        {
            tr = rootObject;
            rigid = rootObject.GetComponent<Rigidbody>();
            seeker = GetComponent<Seeker>();
            rvoController = GetComponent<RVOController>();
            controller = GetComponent<CharacterController>();
            rigid2D = GetComponent<Rigidbody2D>();
        }
    }
}


