using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

namespace Common.Systems
{
    public class PathfindingSystem : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform rootTransform;
        [SerializeField] private Seeker agent;
        [SerializeField] private AIMove aiMove;
        [SerializeField] private LayerMask environmentLayer;

        public bool IsReached => aiMove.reachedEndOfPath;
        public bool IsStopped => aiMove.isStopped;
        public bool IsSafe => PathfindingUtility.IsSafePosition(rootTransform.position);
        public bool CanMove { get => aiMove.canMove; set => aiMove.canMove = value; }
        public Vector3 Direction => rootTransform.forward;

        // TODO. KnockBack Property. 본래 Sequencer에서 해야하지만, 3 Parameter 만들기가 너무 귀찮아서 임시 방편.
        private float KnockBackDistance { get; set; } = 1f;
        private float KnockBackDuration { get; set; } = 1f;
        private Vector3 RootPosition => rootTransform.position;


        public void Move(Vector3 destination, Action callback)
        {
            aiMove.updateRotation = true;
            aiMove.maxSpeed       = moveSpeed;

            if (callback != null)
            {
                aiMove.Callback -= callback;
                aiMove.Callback += callback;
            }

            agent.StartPath(RootPosition, destination);
        }

        public async UniTask Move(Vector3 destination)
        {
            aiMove.updateRotation = true;
            aiMove.maxSpeed       = moveSpeed;
            agent.StartPath(RootPosition, destination);

            await UniTask.WaitUntil(() => IsReached);
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

        public void Dash(Vector3 direction, float distance, Action callback)
        {
            var dashDestination = PathfindingUtility.GetReachableStraightPosition(RootPosition, direction, distance);

            rootTransform.DOMove(dashDestination, 0.15f).OnComplete(() => callback?.Invoke());
        }

        public void KnockBack(Vector3 from, float distance, float duration, Action callback)
        {
            var knockBackDirection = RootPosition - from;
            var knockBackDestination = PathfindingUtility.GetReachableStraightPosition(RootPosition, knockBackDirection, distance);

            rootTransform.DOMove(knockBackDestination, duration)
                         .OnComplete(() => callback?.Invoke());
        }

        public void SetKnockProperty(float distance, float duration)
        {
            KnockBackDistance = distance;
            KnockBackDuration = duration;
        }
        
        public async UniTask KnockBack(Vector3 source)
        {
            var knockBackDirection = RootPosition - source;
            var knockBackDestination = PathfindingUtility.GetReachableStraightPosition(RootPosition, knockBackDirection, KnockBackDistance);

            await rootTransform.DOMove(knockBackDestination, KnockBackDuration);
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
