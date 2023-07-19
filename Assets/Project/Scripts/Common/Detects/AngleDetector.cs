using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Detects
{
    public class AngleDetector : ITakerDetection
    {
        private Collider[] Buffer { get; } = new Collider[32];
        private List<ICombatTaker> TakerList { get; } = new(32);

        
        public List<ICombatTaker> GetTakers(Vector3 pivot, Vector3 forward, LayerMask layerMask, SizeEntity entity)
        {
            return GetTargetsInAngle(pivot, forward, layerMask, entity.AreaRange, entity.Angle);
        }
        
        
        private List<ICombatTaker> GetTargetsInAngle(Vector3 center, Vector3 forward, LayerMask layer, float radius, float angle)
        {
            Array.Clear(Buffer, 0, Buffer.Length);
            
            if (Physics.OverlapSphereNonAlloc(center, radius, Buffer, layer) == 0) return null;

            TakerList.Clear();
            Buffer.ForEach(collider =>
            {
                if (collider.IsNullOrEmpty() || 
                    !collider.TryGetComponent(out ICombatTaker taker) || 
                    !taker.Alive.Value) return;
                
                if (Math.Abs(angle - 345.0f) > 0.000001f)
                {
                    var direction = (collider.transform.position - center).normalized;
        
                    if (Vector3.Angle(forward, direction) > angle * 0.5f) return;
                }
                
                TakerList.Add(taker);
            });
        
            return TakerList;
        }
    }
}
