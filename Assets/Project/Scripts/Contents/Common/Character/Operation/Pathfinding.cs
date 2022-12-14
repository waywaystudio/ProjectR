using Pathfinding;
using UnityEngine;

namespace Common.Character.Operation
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        
        private AIMove aiMove;
        private Seeker agent;
        private int instanceID;

        public bool HasPath => aiMove.hasPath;
        public bool IsReached => aiMove.reachedEndOfPath;

        public void Move(Vector3 destination)
        {
            var abPath = ABPath.Construct(cb.transform.position, destination);

            aiMove.maxSpeed = cb.MoveSpeed.ResultToFloat;
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
            instanceID = GetInstanceID();
        }

        private void OnEnable()
        {
            cb.IsReached.Register(instanceID, () => IsReached);
            cb.OnWalk.Register(instanceID, Move);
            cb.OnRun.Register(instanceID, Move);
        }

        private void OnDisable()
        {
            cb.IsReached.UnRegister(instanceID);
            cb.OnWalk.UnRegister(instanceID);
            cb.OnRun.UnRegister(instanceID);
        }
    }
}