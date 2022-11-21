using UnityEngine;

namespace Pathfinding
{
    using RVO;
    
    /// <summary>
    /// Inherit AIPath Class for Custom Hierarchy by Override FindComponents. 
    /// </summary>
    public class AIMove : AIPath
    {
        private Transform rootObject;
        private Seeker agent;
        private Rigidbody rigidBody;

        public void Initialize(Transform rootObject, Seeker agent, Rigidbody rigidBody)
        {
            this.rootObject = rootObject;
            this.agent = agent;
            this.rigidBody = rigidBody;
        }

        public override void FindComponents()
        {
            tr = rootObject;
            rigid = rigidBody;
            seeker = agent ??= GetComponent<Seeker>();
            rvoController = GetComponent<RVOController>();
            controller = GetComponent<CharacterController>();
            rigid2D = GetComponent<Rigidbody2D>();
        }
    }
}


