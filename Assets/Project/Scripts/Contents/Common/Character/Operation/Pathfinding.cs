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

        public Vector3 Direction
        {
            get
            {
                // TODO. Direction 이 제대로 작동되는 것을 확인하기 전까지는 Debug를 띄우자.
                // return !IsReached 
                //         ? (aiMove.steeringTarget - cb.transform.position).normalized
                //         : cb.MainTarget != default
                //             ? (cb.MainTarget.Taker.transform.position - cb.transform.position).normalized
                //             : Vector3.zero;
                if (!IsReached)
                {
                    return (aiMove.steeringTarget - cb.transform.position).normalized;
                }

                if (cb.MainTarget != default)
                {
                    return (cb.MainTarget.Taker.transform.position - cb.transform.position).normalized;
                }

                Debug.LogWarning("Hmm...Where is Target?");
                return Vector3.zero;
            }
        }
            

        public void Move(Vector3 destination)
        {
            var abPath = ABPath.Construct(cb.transform.position, destination);

            aiMove.maxSpeed = cb.MoveSpeed.Result;
            agent.StartPath(abPath);
        }

        public void Stop()
        {
            // Destination = rootObject.position; 경우에 Flip 덜덜이 발생함.
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
            cb.Direction.Register(instanceID, () => Direction);
            cb.IsReached.Register(instanceID, () => IsReached);
            cb.OnWalk.Register(instanceID, Move);
            cb.OnRun.Register(instanceID, Move);
        }

        private void OnDisable()
        {
            cb.Direction.UnRegister();
            cb.IsReached.UnRegister(instanceID);
            cb.OnWalk.UnRegister(instanceID);
            cb.OnRun.UnRegister(instanceID);
        }
    }
}