using Pathfinding;
using UnityEngine;

namespace Common.Character.Operation
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        
        private AIMove aiMove;
        private Seeker agent;

        public bool HasPath => aiMove.hasPath;
        public bool IsReached => aiMove.reachedEndOfPath;

        public void Move(Vector3 destination)
        {
            var abPath = ABPath.Construct(cb.transform.position, destination);

            aiMove.maxSpeed = cb.MoveSpeed;
            agent.StartPath(abPath);
        }

        public void Stop()
        {
            // Destination = rootObject.position;
            // 위 경우에 Flip 덜덜이 발생함.
            aiMove.destination = aiMove.steeringTarget;
        }
        

        private void Awake()
        {
            agent = GetComponent<Seeker>();
            aiMove = GetComponent<AIMove>();
        }
    }
}