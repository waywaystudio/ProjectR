using System;
using Core;
using Pathfinding;
using UnityEngine;

namespace Character.PathfindingSystem
{
    public class Pathfinding : MonoBehaviour, IPathfinding, IEditorSetUp
    {
        [SerializeField] private AIMove aiMove;
        [SerializeField] private Seeker agent;

        private CharacterBehaviour cb;
        private ABPath pathBuffer;
        private int instanceID;

        public bool IsReached => aiMove.reachedEndOfPath;

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
            TryGetComponent(out agent);
            TryGetComponent(out aiMove);
            
            cb                   ??= GetComponentInParent<CharacterBehaviour>();
            instanceID           =   GetInstanceID();
            cb.PathfindingEngine =   this;
        }

        private void OnEnable()
        {
            cb.OnTeleport.Register(instanceID, TeleportTo);
            cb.OnWalk.Register(instanceID, Move);
            cb.OnRun.Register(instanceID, Move);
        }

        private void OnDisable()
        {
            cb.OnTeleport.Unregister(instanceID);
            cb.OnWalk.Unregister(instanceID);
            cb.OnRun.Unregister(instanceID);
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