using Core;
using Pathfinding;
using UnityEngine;

namespace Character.PathfindingSystem
{
    public class Pathfinding : MonoBehaviour, IPathfinding
    {
        [SerializeField] private Transform rootTransform; 
        [SerializeField] private AIMove aiMove;
        [SerializeField] private Seeker agent;
        
        private ABPath pathBuffer;
        private float moveSpeed;
        
        public bool IsReached => aiMove.reachedEndOfPath;
        public bool IsSafe => AstarPath.active.GetNearest(rootPosition, NNConstraint.None).node.Walkable;
        public Vector3 Direction { get; }

        private Vector3 rootPosition => rootTransform.position;
        
        // public void Move(Vector3 destination)
        // {
        //     pathBuffer      = ABPath.Construct(rootPosition, destination);
        //     aiMove.maxSpeed = cb.StatTable.MoveSpeed;
        //
        //     if (aiMove.Callback != null || callback != null) aiMove.Callback = callback;
        //
        //     agent.StartPath(pathBuffer);
        // }

        // public void Move(Vector3 destination, bool ignoreObstacle, Action callback) { }
    }
}
