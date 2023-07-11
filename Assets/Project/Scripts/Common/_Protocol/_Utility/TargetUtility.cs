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
    }
}
