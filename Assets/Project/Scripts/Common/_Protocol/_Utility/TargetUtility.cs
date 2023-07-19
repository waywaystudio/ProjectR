using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class TargetUtility
    {
        public static List<T> ConvertTargets<T>(List<GameObject> originalList)
        {
            var result = new List<T>();
            
            originalList?.ForEach(component =>
            {
                if (component.IsNullOrDestroyed()) return;
                if (component.TryGetComponent(out T element))
                {
                    result.Add(element);
                }
            });

            return result;
        }


        public static Vector3 GetValidPosition(Vector3 center, float range, Vector3 targetPosition)
        {
            if (Vector3.Distance(center, targetPosition) <= range)
            {
                return targetPosition;
            }

            var direction = (targetPosition - center).normalized;
            var destination = center + direction * range;

            return destination;
        } 
        
        
        public static List<T> GetTargetsInSphere<T>(Vector3 center, LayerMask layer, float radius, Collider[] buffer) 
            => GetTargetsInAngle<T>(center, Vector3.zero, layer, radius, 360f, buffer);


        /// <summary>
        /// Detector와 같은 클래스에서 사용하기 위해 만들어졌다.
        /// 매 함수에 파라메타가 많기 때문에, 타 클래스에서 직접적으로 사용하기 보다는 Detector 클래서에서 사용하는 것을 권장 한다.
        /// </summary>
        public static List<T> GetTargetsInAngle<T>(Vector3 center, Vector3 forward, LayerMask layer, float radius, float angle, Collider[] buffer)
        {
            Array.Clear(buffer, 0, buffer.Length);
            
            if (Physics.OverlapSphereNonAlloc(center, radius, buffer, layer) == 0) return null;

            var result = new List<T>();
            
            buffer.ForEach(collider =>
            {
                if (collider.IsNullOrEmpty() || !collider.TryGetComponent(out T taker)) return;
                
                if (Math.Abs(angle - 360.0f) > 0.000001f)
                {
                    var direction = (collider.transform.position - center).normalized;
        
                    if (Vector3.Angle(forward, direction) > angle * 0.5f) return;
                }
                
                result.Add(taker);
            });
        
            return result;
        }
        
        public static List<T> GetTargetsInRect<T>(Vector3 pivot, Vector3 forward, LayerMask layer, float width, float height, Collider[] buffer)
        {
            Array.Clear(buffer, 0, buffer.Length);

            var center = pivot + forward * (height * 0.5f);
            var halfExtents = new Vector3(width * 0.5f, height * 0.5f, height * 0.5f);

            // Rotate the forward vector 90 degrees around the Y axis for the "right" direction.
            var right = Quaternion.Euler(0, 90, 0) * forward;
            var orientation = Quaternion.LookRotation(forward, Vector3.up);
            var size = Physics.OverlapBoxNonAlloc(center, halfExtents, buffer, orientation, layer);
            var result = new List<T>();

            for (var i = 0; i < size; i++)
            {
                var collider = buffer[i];

                if (collider is null || !collider.TryGetComponent(out T taker)) continue;

                var projectedPosition = Vector3.ProjectOnPlane(collider.transform.position - pivot, Vector3.up);
                var toProjectedPosition = projectedPosition - Vector3.zero;

                if (Vector3.Dot(toProjectedPosition, forward) <= height &&
                    Vector3.Dot(toProjectedPosition, right)   <= width * 0.5f)
                    
                    result.Add(taker);
            }

            return result;
        }
        
        public static T GetTakerInWidthRaycast<T>(Vector3 pivot, Vector3 forward, LayerMask layer, float width, float height)
        {
            // Modify pivot's Y value
            pivot.y += 1f;
    
            // Create a halfExtents vector with half of width and height
            var halfExtents = new Vector3(width * 0.5f, height * 0.5f, 0f);
    
            // Set up the orientation of the box
            var orientation = Quaternion.LookRotation(forward);
    
            // Do the BoxCast
            if (Physics.BoxCast(pivot, halfExtents, forward, out var hit, orientation, Mathf.Infinity, layer))
            {
                // If the collider hit has the component T, return it
                if (hit.collider.TryGetComponent(out T taker))
                {
                    return taker;
                }
            }

            // Return default if no suitable collider was hit
            return default;
        }
        
        public static List<T> GetTakersInWidthRaycast<T>(Vector3 pivot, Vector3 forward, RaycastHit[] buffer, LayerMask layer, float width, float height, int pierceCount)
        {
            // Modify pivot's Y value
            pivot.y += 1f;

            // Create a halfExtents vector with half of width and height
            var halfExtents = new Vector3(width / 2f, height / 2f, 0f);

            // Set up the orientation of the box
            var orientation = Quaternion.LookRotation(forward);

            // Do the BoxCast
            var hitCount = Physics.BoxCastNonAlloc(pivot, halfExtents, forward, buffer, orientation, Mathf.Infinity, layer);

            var takers = new List<T>();
            var hitDistanceMap = new List<KeyValuePair<T, float>>();
            for (var i = 0; i < hitCount; i++)
            {
                var hit = buffer[i];
                if (hit.collider.TryGetComponent(out T taker))
                {
                    // ICombatTaker의 경우 Alive 여부 판단 필요.
                    hitDistanceMap.Add(new KeyValuePair<T, float>(taker, hit.distance));
                }
            }

            // Clear unused entries in buffer
            for (var i = hitCount; i < buffer.Length; i++) buffer[i] = default;

            hitDistanceMap.Sort((a, b) => a.Value.CompareTo(b.Value));

            // If pierceCount is 0, return only the nearest taker
            if (pierceCount == 0 && hitDistanceMap.Count > 0)
                takers.Add(hitDistanceMap[0].Key);
            else
                // Return the 'pierceCount' number of nearest takers
                for (var i = 0; i < pierceCount && i < hitDistanceMap.Count; i++)
                    takers.Add(hitDistanceMap[i].Key);

            return takers;
        }


    }
}
