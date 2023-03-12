using System;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

namespace Common.Systems
{
    public class PathfindingSystem : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float raycastThreshHold = 2f;
        [SerializeField] private float knockBackDuration = 0.35f;
        [SerializeField] private Transform rootTransform;
        [SerializeField] private Seeker agent;
        [SerializeField] private AIMove aiMove;
        [SerializeField] private LayerMask environmentLayer;

        public bool IsReached => aiMove.reachedEndOfPath;
        public bool IsSafe => PathfindingUtility.IsSafePosition(rootTransform.position);
        public bool CanMove { get => aiMove.canMove; set => aiMove.canMove = value; }
        public Vector3 Direction => rootTransform.forward;

        private Vector3 rootPosition => rootTransform.position;


        public void Move(Vector3 destination, Action callback)
        {
            aiMove.updateRotation = true;
            aiMove.maxSpeed       = moveSpeed;

            if (callback != null)
            {
                aiMove.Callback -= callback;
                aiMove.Callback += callback;
            }

            agent.StartPath(rootPosition, destination);
        }

        public void RigidRotate(Vector3 lookTarget)
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

        public void Jump(Vector3 direction, float availableDistance, float jumpPower = 2.4f, float duration = 0.77f)
        {
            var normalDirection = direction.normalized;
            var actualDistance  = availableDistance;
            
            if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, availableDistance + 2f, environmentLayer))
            {
                actualDistance = hitInfo.distance - raycastThreshHold;
            }

            rootTransform.DOJump(rootPosition + normalDirection * actualDistance, jumpPower, 1, duration)
                         .SetEase(Ease.InSine);
        }
        

        public void Dash(Vector3 direction, float distance, Action callback)
        {
            var normalDirection = direction.normalized;
            var actualDistance = distance;
            
            if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, distance + 2f, environmentLayer))
            {
                actualDistance = hitInfo.distance - raycastThreshHold;
            }

            rootTransform.DOMove(rootPosition + normalDirection * actualDistance, 0.15f).OnComplete(() => callback?.Invoke());
        }

        public void Teleport(Vector3 direction, float distance, Action callback)
        {
            var normalDirection   = direction.normalized;
            var targetDestination = rootPosition + normalDirection * distance;

            if (!PathfindingUtility.IsPathPossible(rootPosition, targetDestination))
            {
                if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, distance + 2f, environmentLayer))
                {
                    distance          = hitInfo.distance - raycastThreshHold;
                    targetDestination = rootPosition     + normalDirection * distance;
                }
                else
                {
                    targetDestination = PathfindingUtility.GetNearestSafePathNode(rootTransform.position, targetDestination);
                }
            }

            aiMove.Teleport(targetDestination);
            callback?.Invoke();
        }

        public void KnockBack(Vector3 from, float distance, Action callback)
        {
            var knockBackDirection = (rootPosition - from).normalized;
            var knockBackDestination = rootPosition + knockBackDirection * distance;

            if (Physics.Raycast(rootPosition, knockBackDestination, out var hitInfo, distance, environmentLayer))
            {
                distance = hitInfo.distance - raycastThreshHold;
            }

            rootTransform.DOMove(rootPosition + knockBackDirection * distance, knockBackDuration)
                         .OnComplete(() => callback?.Invoke());
        }

        public void Quit()
        {
            Stop();
            enabled = false;
        }

        public void TraverseToNonObstacle() => agent.graphMask = GraphMask.FromGraphName("Non Obstacle Graph");
        public void TraverseToDefault() => agent.graphMask = GraphMask.FromGraphName("Default Grid Graph");
    }
}
