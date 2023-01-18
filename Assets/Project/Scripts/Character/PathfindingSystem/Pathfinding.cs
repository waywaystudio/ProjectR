using System;
using Core;
using Pathfinding;
using UnityEngine;

namespace Character.PathfindingSystem
{
    public class Pathfinding : MonoBehaviour, IPathfinding, IInspectorSetUp
    {
        [SerializeField] private AIMove aiMove;
        [SerializeField] private Seeker agent;

        private CharacterBehaviour cb;
        private ABPath pathBuffer;
        private int instanceID;

        public bool IsReached => aiMove.reachedEndOfPath;
        public bool IsSafe => AstarPath.active.GetNearest(cb.transform.position, NNConstraint.None).node.Walkable;
        public Vector3 Direction
        {
            get
            {
                if (!IsReached) return (aiMove.steeringTarget - cb.transform.position).normalized;
                if (cb.TargetingEngine.MainTarget != null)
                    return (cb.TargetingEngine.MainTarget.Object.transform.position - cb.transform.position).normalized;

                if (cb.SearchingEngine.LookTarget != null)
                    return (cb.SearchingEngine.LookTarget.Object.transform.position - cb.transform.position).normalized;

                Debug.LogWarning("Hmm...Where is Target?");
                return Vector3.zero;
            }
        }

        public void Move(Vector3 destination, Action callback)
        {
            pathBuffer      = ABPath.Construct(cb.transform.position, destination);
            aiMove.maxSpeed = cb.StatTable.MoveSpeed;

            if (aiMove.Callback != null || callback != null) aiMove.Callback = callback;

            agent.StartPath(pathBuffer);
        }

        public void TeleportTo(Vector3 destination) { aiMove.Teleport(destination); }

        public void Stop()
        {
            // Destination = rootObject.position; 경우에 Flip 덜덜이 발생함.
            // aiMove.destination = aiMove.steeringTarget;
            aiMove.destination = cb.transform.position;
        }
        

        private void Awake()
        {
            aiMove     ??= GetComponent<AIMove>();
            agent      ??= GetComponent<Seeker>();
            cb         ??= GetComponentInParent<CharacterBehaviour>();
            instanceID =   GetInstanceID();
            
            cb.OnTeleport.Register(instanceID, TeleportTo);
            cb.OnWalk.Register(instanceID, Move);
            cb.OnRun.Register(instanceID, Move);
        }


#if UNITY_EDITOR
        public void SetUp()
        {
            TryGetComponent(out agent);
            TryGetComponent(out aiMove);
        }
#endif
    }
}