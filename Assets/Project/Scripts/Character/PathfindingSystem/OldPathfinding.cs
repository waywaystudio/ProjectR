using System;
using Core;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.PathfindingSystem
{
    public class OldPathfinding : MonoBehaviour, IPathfinding, IInspectorSetUp
    {
        [SerializeField] private AIMove aiMove;
        [SerializeField] private Seeker agent;

        private CharacterBehaviour cb;
        // private ABPath pathBuffer;
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
            // aiMove.isStopped = false;
            aiMove.maxSpeed  = cb.StatTable.MoveSpeed;
            agent.StartPath(cb.transform.position, destination);

            if (callback != null)
            {
                aiMove.Callback -= callback;
                aiMove.Callback += callback;
            }
        }

        public void TeleportTo(Vector3 destination) { aiMove.Teleport(destination); }

        [Button]
        public void Stop()
        {
            aiMove.SetPath(null);
            
            cb.Idle();
        }

        public void DisableMove()
        {
            aiMove.canMove = false;
        }
        

        private void Awake()
        {
            aiMove     ??= GetComponent<AIMove>();
            agent      ??= GetComponent<Seeker>();
            cb         ??= GetComponentInParent<CharacterBehaviour>();
            instanceID =   GetInstanceID();
            
            cb.OnTeleport.Register(instanceID, TeleportTo);
            cb.OnWalk.Register(instanceID, Move);
            // cb.OnRun.Register(instanceID, (x) => Move(x, () => Debug.Log("Movement Callback")));
            cb.OnRun.Register(instanceID, Move);
            cb.OnDead.Register(instanceID, DisableMove);
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