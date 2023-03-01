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

        public void RotateTo(Vector3 lookTarget)
        {
            // TODO. LookAt과 A* Path의 updateRotation이 충돌한다. 좋은 방법을 찾아보자.
            aiMove.updateRotation = false;
            rootTransform.LookAt(lookTarget);
        }

        public void Stop()
        {
            aiMove.SetPath(null);
            aiMove.destination = Vector3.positiveInfinity;
        }

        public void Jump(Vector3 direction, float availableDistance)
        {
            Stop();
            RotateTo(rootTransform.position + direction);

            var normalDirection = direction.normalized;
            var actualDistance  = availableDistance;
            
            if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, availableDistance + 2f, environmentLayer))
            {
                // TODO. 2f to ThreshHold
                actualDistance = hitInfo.distance - 2f;
            }
            
            // Value Designed for Knight ThunderClap
            rootTransform.DOJump(rootPosition + normalDirection * actualDistance, 2.4f, 1, 0.77f).
                          SetEase(Ease.InSine);
        }
        

        public void Dash(Vector3 direction, float distance, Action callback)
        {
            Stop();
            RotateTo(rootTransform.position + direction);

            var normalDirection = direction.normalized;
            var actualDistance = distance;
            
            if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, distance + 2f, environmentLayer))
            {
                // TODO. 2f to ThreshHold
                actualDistance = hitInfo.distance - 2f;
            }

            rootTransform.DOMove(rootPosition + normalDirection * actualDistance, 0.15f).OnComplete(() => callback?.Invoke());
        }

        public void Teleport(Vector3 direction, float distance, Action callback)
        {
            Stop();
            RotateTo(rootTransform.position + direction);

            var normalDirection   = direction.normalized;
            var targetDestination = rootPosition + normalDirection * distance;

            if (!PathfindingUtility.IsPathPossible(rootPosition, targetDestination))
            {
                if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, distance + 2f, environmentLayer))
                {
                    // TODO. 2f to ThreshHold
                    distance            = hitInfo.distance - 2f;
                    targetDestination = rootPosition + normalDirection * distance;
                }
                else
                {
                    targetDestination = PathfindingUtility.GetNearestSafePathNode(rootTransform.position, targetDestination);
                }
            }

            aiMove.Teleport(targetDestination);
            callback?.Invoke();
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
