using System;
using Core;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

namespace Character.Move
{
    public class MoveBehaviour : MonoBehaviour, IPathfinding
    {
        [SerializeField] private Transform rootTransform;
        [SerializeField] private Seeker agent;
        [SerializeField] private AIMove aiMove;
        [SerializeField] private float moveSpeed;
        [SerializeField] private LayerMask environmentLayer;

        public bool IsMovable => aiMove.canMove;
        public bool IsReached => aiMove.reachedEndOfPath;
        public bool IsSafe => PathfindingUtility.IsSafePosition(rootTransform.position);
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
        
        public void Rotate(Vector3 lookTarget) => rootTransform.LookAt(lookTarget);

        public void Stop()
        {
            aiMove.SetPath(null);
            aiMove.destination = Vector3.positiveInfinity;
        }

        public void Dash(Vector3 direction, Action callback)
        {
            Stop();
            Rotate(direction);

            var normalDirection = direction.normalized;
            var dashDestination = rootPosition + normalDirection * 8f;
            var distance = Vector3.Distance(dashDestination, rootPosition);
            
            if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, distance + 2f, environmentLayer))
            {
                // TODO. 2f to ThreshHold
                distance = hitInfo.distance - 2f;
            }

            rootTransform.DOMove(rootPosition + normalDirection * distance, 0.15f).OnComplete(callback.Invoke);
        }

        public void Teleport(Vector3 direction)
        {
            Stop();
            Rotate(direction);

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
            Rotate(from);
            
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
