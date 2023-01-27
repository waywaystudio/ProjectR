using Core;
using Pathfinding;
using UnityEngine;

namespace Character
{
    public static class PathfindingUtility
    {      
        public static Vector3 GetNearestSafePosition(Vector3 point, float offsetDistance)
        {
            var startNode = AstarPath.active.GetNearest(point, NNConstraint.Default).node;
            var nodes = PathUtilities.BFS(startNode, 0);
            var singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0];
            var offset = (singleRandomPoint - point).normalized * offsetDistance;
            
            return singleRandomPoint + offset;
        }

        public static bool TryGetCombatPosition(ICombatProvider provider, ICombatTaker taker, float range, out Vector3 combatPosition)
        {
            var target = taker.Object;
            var targetPosition    = target.transform.position;
            var characterPosition = provider.Object.transform.position;
            
            var currentDistance = Vector3.Distance(targetPosition, characterPosition);
            float magnitude;
            Vector3 direction;
            
            // Adventurer 고유 값으로 대체
            var safeTolerance = 1f;
            var safePoint = 1f;
            
            if (currentDistance > range)
            {
                direction = (targetPosition - characterPosition).normalized;
                magnitude = Mathf.Abs(currentDistance - range) + range * (safeTolerance * safePoint);
            }
            else
            {
                combatPosition = characterPosition;
                return false;
            }
            
            combatPosition = characterPosition + direction * magnitude;
            return true;
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