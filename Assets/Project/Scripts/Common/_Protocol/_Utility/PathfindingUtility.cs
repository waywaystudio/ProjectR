using Pathfinding;
using UnityEngine;

namespace Common
{
    public static class PathfindingUtility
    {
        public static bool IsSafePosition(Vector3 position)
        {
            var node = AstarPath.active.GetNearest(position, NNConstraint.None).node;

            return node.Walkable;
        }

        public static bool IsPathPossible(Vector3 from, Vector3 dest)
        {
            var fromNode = AstarPath.active.GetNearest(from, NNConstraint.None).node;
            var destNode = AstarPath.active.GetNearest(dest, NNConstraint.None).node;

            return PathUtilities.IsPathPossible(fromNode, destNode);
        }
        
        /// <summary>
        /// Get just Nearest Walkable Node of point. wherever you are; 
        /// </summary>
        /// <returns>Nearest Walkable Node.Position</returns>
        public static Vector3 GetNearestSafePosition(Vector3 point, float offsetDistance)
        {
            var startNode = AstarPath.active.GetNearest(point, NNConstraint.Default).node;
            var nodes = PathUtilities.BFS(startNode, 0);
            var singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0];
            var offset = (singleRandomPoint - point).normalized * offsetDistance;
            
            return singleRandomPoint + offset;
        }
        
        /// <summary>
        /// Get Nearest Walkable Node from destination via Path to pivot.  
        /// </summary>
        /// <returns>Nearest Walkable Node.Position</returns>
        public static Vector3 GetNearestSafePathNode(Vector3 pivot, Vector3 destination)
        {
            var targetPath = ABPath.Construct(pivot, destination);
            var nNode = pivot;

            targetPath.path.ForEach(node => node.Walkable.OnTrue(() => nNode = (Vector3)node.position));

            return nNode;
        }

        public static bool TryGetCombatPosition(ICombatProvider provider, ICombatTaker taker, float range, out Vector3 combatPosition)
        {
            var target = taker.gameObject;
            var targetPosition    = target.transform.position;
            var characterPosition = provider.gameObject.transform.position;
            
            var currentDistance = Vector3.Distance(targetPosition, characterPosition);

            // Adventurer 고유 값으로 대체
            var safeTolerance = 1.1f;
            
            if (currentDistance > range)
            {
                var direction = (targetPosition - characterPosition).normalized;
                var magnitude = Mathf.Abs(currentDistance - range) + range * safeTolerance;
                
                combatPosition = characterPosition + direction * magnitude;
            }
            else
            {
                combatPosition = characterPosition;
                return false;
            }

            return true;
        }
        
        public static bool IsGround(Vector3 position, out Vector3 groundPosition)
        {
            var ray = new Ray(new Vector3(position.x, 100f, position.z), Vector3.down);

            if (Physics.Raycast(ray, out var hit, 100f * 2, LayerMask.GetMask("Ground")))
            {
                groundPosition = hit.point;
                return true;
            }

            groundPosition = Vector3.positiveInfinity;
            return false;
        }

        public static Vector3 GetReachableStraightPosition(Vector3 root, Vector3 direction, float distance)
        {
            // 1. 땅이 있어야 하고.
            // 2. 캐릭터로부터 원하는 지점에 Raycast를 쐈을 때, 충돌하는 것이 없어야 함.
            if (distance < 0) return root;
            
            var normalDirection = direction.normalized;

            while (distance > 0)
            {
                var targetPosition = root + normalDirection * distance;
                var noObstacle = !Physics.Raycast(root, normalDirection, out _, distance, LayerMask.GetMask("Environment"));
                var isGround = IsGround(targetPosition, out var groundPosition);

                if (noObstacle && isGround)
                {
                    return groundPosition;
                }
                
                distance -= 1.0f;

                if (distance < 0f)
                {
                    return root;
                }
            }

            return root;
        }
    }
}

/*
 * 최소 사거리에서 멀어질 경우 참조.
 */
// public bool TryGetCombatPosition(ICombatTaker taker, float range, out Vector3 combatPosition)
//         {
//             var target = taker?.Taker;
//
//             if (target.IsNullOrEmpty())
//             {
//                 Debug.Log("Target is Null");
//                 combatPosition = Vector3.negativeInfinity;
//                 return false;
//             }
//             
//             targetPosition = target.transform.position;
//             characterPosition = transform.position;
//             
//             var currentDistance = Vector3.Distance(targetPosition, characterPosition);
//             float magnitude;
//             
//             //--outOfRange - MaxRange -<            SafeRange          >- CloseRange - Target
//             //--------------------------safeRange.x----------safeRange.y---------------Target
//             //------------------------------------safeTolerance------------------------Target
//             //---------Player----------------------------------------------------------Target
//             // case Out of Range
//             if (currentDistance > range)
//             {
//                 direction = (targetPosition - characterPosition).normalized;
//                 magnitude = Mathf.Abs(currentDistance - range) + range * (safeTolerance * safePoint);
//             }
//             
//             //--outOfRange - MaxRange -<            SafeRange          >- CloseRange - Target
//             //--------------------------safeRange.x----------safeRange.y---------------Target
//             //------------------------------------safeTolerance------------------------Target
//             //--------------------------------------------------------------Player-----Target
//             // case Too Close Range
//             else if (currentDistance <= range * (1.0f - safeTolerance))
//             {
//                 direction = (targetPosition - characterPosition).normalized * -1.0f;
//                 magnitude = Mathf.Abs(currentDistance - range) - range * (safeTolerance * safePoint);
//             }
//             
//             //--outOfRange - MaxRange -<            SafeRange          >- CloseRange - Target
//             //--------------------------safeRange.x----------safeRange.y---------------Target
//             //------------------------------------safeTolerance------------------------Target
//             //-----------------------------------Player--------------------------------Target
//             // case In Safe Range
//             else
//             {
//                 combatPosition = characterPosition;
//                 return false;
//             }
//
//             combatPosition = characterPosition + direction * magnitude;
//             return true;
//         }
