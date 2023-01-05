using System;
using Pathfinding;
using UnityEngine;

namespace Character.Pathfinding
{
    public class Pathfinding : MonoBehaviour
    {
        private CharacterBehaviour cb;
        private AIMove aiMove;
        private Seeker agent;
        private ABPath pathBuffer;
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
                    return (cb.MainTarget.Object.transform.position - cb.transform.position).normalized;
                }

                Debug.LogWarning("Hmm...Where is Target?");
                return Vector3.zero;
            }
        }
            

        public void Move(Vector3 destination, Action callback)
        {
            pathBuffer = ABPath.Construct(cb.transform.position, destination);
            aiMove.maxSpeed = cb.StatTable.MoveSpeed;

            if (aiMove.Callback != null || callback != null)
            {
                aiMove.Callback = callback;
            }

            agent.StartPath(pathBuffer);
        }

        public void TeleportTo(Vector3 destination)
        {
            aiMove.Teleport(destination);
        }

        public void Stop()
        {
            // Destination = rootObject.position; 경우에 Flip 덜덜이 발생함.
            // aiMove.destination = aiMove.steeringTarget;
            aiMove.destination = cb.transform.position;
        }


        private void Awake()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            
            agent = GetComponent<Seeker>();
            aiMove = GetComponent<AIMove>();
            instanceID = GetInstanceID();
        }

        private void OnEnable()
        {
            cb.Direction.Register(instanceID, () => Direction);
            cb.IsReached.Register(instanceID, () => IsReached);
            cb.OnTeleport.Register(instanceID, TeleportTo);
            cb.OnWalk.Register(instanceID, Move);
            cb.OnRun.Register(instanceID, Move);
        }

        private void OnDisable()
        {
            cb.Direction.UnregisterAll();
            cb.IsReached.Unregister(instanceID);
            cb.OnTeleport.Unregister(instanceID);
            cb.OnWalk.Unregister(instanceID);
            cb.OnRun.Unregister(instanceID);
        }
    }
}