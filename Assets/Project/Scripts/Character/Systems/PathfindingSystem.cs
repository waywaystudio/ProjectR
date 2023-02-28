using System;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

namespace Character.Systems
{
    public class PathfindingSystem : MonoBehaviour
    {
        [SerializeField] private Transform rootTransform;
        [SerializeField] private Seeker agent;
        [SerializeField] private AIMove aiMove;
        [SerializeField] private float moveSpeed;
        [SerializeField] private LayerMask environmentLayer;

        // public bool IsMovable => aiMove.canMove;
        public bool IsReached => aiMove.reachedEndOfPath;
        public bool IsSafe => PathfindingUtility.IsSafePosition(rootTransform.position);
        public bool CanMove { get => aiMove.canMove; set => aiMove.canMove = value; }
        public Vector3 Direction => rootTransform.forward;

        private Vector3 rootPosition => rootTransform.position;


        public void Move(Vector3 destination, Action callback)
        {
            aiMove.maxSpeed = moveSpeed;

            if (callback != null)
            {
                aiMove.Callback -= callback;
                aiMove.Callback += callback;
            }

            agent.StartPath(rootPosition, destination);
        }
        
        public void RotateTo(Vector3 lookTarget) => rootTransform.LookAt(lookTarget);

        public void Stop()
        {
            aiMove.SetPath(null);
            aiMove.destination = Vector3.positiveInfinity;
        }

        public void Jump(Vector3 destination, float availableDistance, Action callback = null)
        {
            Stop();
            RotateTo(destination);

            var normalDirection = (destination - rootPosition).normalized;
            var actualDistance  = availableDistance;
            
            if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, availableDistance + 2f, environmentLayer))
            {
                // TODO. 2f to ThreshHold
                actualDistance = hitInfo.distance - 2f;
            }
            
            // Value Designed for Knight ThunderClap
            rootTransform.DOLocalJump(rootPosition + normalDirection * actualDistance, 2.4f, 1, 0.77f)
                         .SetEase(Ease.InSine)
                         .OnComplete(() => callback?.Invoke());
        }
        

        public void Dash(Vector3 direction, float distance, Action callback)
        {
            Stop();
            RotateTo(direction);

            var normalDirection = direction.normalized;
            var actualDistance = distance;
            
            if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, distance + 2f, environmentLayer))
            {
                // TODO. 2f to ThreshHold
                actualDistance = hitInfo.distance - 2f;
            }

            rootTransform.DOMove(rootPosition + normalDirection * actualDistance, 0.15f).OnComplete(callback.Invoke);
        }

        public void Teleport(Vector3 direction)
        {
            Stop();
            RotateTo(direction);

            var normalDirection = direction.normalized;
            var teleportDestination = rootPosition + normalDirection * 8f;
            var distance = Vector3.Distance(teleportDestination, rootPosition);

            if (!PathfindingUtility.IsPathPossible(rootPosition, teleportDestination))
            {
                if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, distance + 2f, environmentLayer))
                {
                    // TODO. 0.5f to ThreshHold
                    distance            = hitInfo.distance - 2f;
                    teleportDestination = rootPosition + normalDirection * distance;
                }
                else
                {
                    teleportDestination = PathfindingUtility.GetNearestSafePathNode(rootTransform.position, teleportDestination);
                }
            }

            aiMove.Teleport(teleportDestination);
        }

        public void KnockBack(Vector3 from, Action callback)
        {
            Stop();
            RotateTo(from);
            
            var knockBackDirection = (rootPosition - from).normalized;
            var knockBackDestination = rootPosition + knockBackDirection * 5f;
            var distance = Vector3.Distance(knockBackDestination, rootPosition);
            
            if (Physics.Raycast(rootPosition, knockBackDestination, out var hitInfo, distance, environmentLayer))
            {
                // TODO. 0.25f to ThreshHold
                distance = hitInfo.distance - 1f;
            }

            rootTransform.DOMove(rootPosition + knockBackDirection * distance, 0.15f).OnComplete(callback.Invoke);
        }
    }
}
