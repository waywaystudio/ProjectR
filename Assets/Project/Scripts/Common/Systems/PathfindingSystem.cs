using System;
using Common.Characters;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Systems
{
    public class PathfindingSystem : MonoBehaviour
    {
        [SerializeField] private float defaultSpeed;
        [SerializeField] private Transform rootTransform;
        [SerializeField] private Seeker agent;
        [SerializeField] private AIMove aiMove;
        [SerializeField] private LayerMask environmentLayer;

        public bool IsReached => aiMove.reachedEndOfPath;
        public bool IsStopped => aiMove.isStopped;
        public bool IsSafe => PathfindingUtility.IsSafePosition(rootTransform.position);
        public bool CanMove { get => aiMove.canMove; set => aiMove.canMove = value; }
        public Vector3 Direction => rootTransform.forward;

        private Vector3 RootPosition => rootTransform.position;
        private Func<float> SpeedRetriever { get; set; }


        public void Move(Vector3 destination, Action callback)
        {
            aiMove.updateRotation = true;
            aiMove.maxSpeed       = SpeedRetriever is not null 
                ? SpeedRetriever.Invoke() 
                : defaultSpeed;

            if (callback != null)
            {
                aiMove.Callback -= callback;
                aiMove.Callback += callback;
            }

            agent.StartPath(RootPosition, destination);
        }

        public void RotateToTarget(Vector3 lookTarget)
        {
            Stop();
            
            aiMove.updateRotation = false;
            rootTransform.LookAt(lookTarget);
        }

        public void Stop()
        {
            aiMove.SetPath(null);
            aiMove.destination = Vector3.positiveInfinity;
        }
        
        // ------------------------------------------
        public void Jump(Vector3 direction, float distance, float jumpPower = 2.4f, float duration = 0.77f)
        {
            var jumpDestination = PathfindingUtility.GetReachableStraightPosition(RootPosition, direction, distance);

            rootTransform.DOJump(jumpDestination, jumpPower, 1, duration)
                         .SetEase(Ease.InSine);
        }

        public void Teleport(Vector3 direction, float distance)
        {
            var destination = PathfindingUtility.GetReachableTeleportPosition(RootPosition, direction, distance);

            aiMove.Teleport(destination);
        }

        public void Dash(Vector3 direction, float distance, float duration, Action callback)
        {
            var dashDestination = PathfindingUtility.GetReachableStraightPosition(RootPosition, direction, distance);

            rootTransform.DOMove(dashDestination, duration).OnComplete(() => callback?.Invoke());
        }

        public void KnockBack(Vector3 from, float distance, float duration, Action callback)
        {
            var knockBackDirection = RootPosition - from;
            var knockBackDestination = PathfindingUtility.GetReachableStraightPosition(RootPosition, knockBackDirection, distance);

            rootTransform.DOMove(knockBackDestination, duration)
                         .OnComplete(() => callback?.Invoke());
        }

        public void Draw(Vector3 dest, float duration, Action callback)
        {
            var drawDirection = dest - RootPosition;
            var drawDistance = Vector3.Distance(dest, RootPosition) * 0.9f;
            
            var drawDestination = PathfindingUtility.GetReachableStraightPosition(RootPosition, drawDirection, drawDistance);
            
            rootTransform.DOMove(drawDestination, duration)
                         .OnComplete(() => callback?.Invoke());
        }

        public void Quit()
        {
            Stop();
            enabled = false;
        }

        public void TraverseToNonObstacle() => agent.graphMask = GraphMask.FromGraphName("Non Obstacle Graph");
        public void TraverseToDefault() => agent.graphMask = GraphMask.FromGraphName("Default Grid Graph");


        private void Awake()
        {
            var cb = GetComponentInParent<CharacterBehaviour>();

            SpeedRetriever = () => cb.StatTable.MoveSpeed;
        }
    }
}
