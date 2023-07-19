using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Detects
{
    public class RectDetector : ITakerDetection
    {
        private Collider[] Buffer { get; } = new Collider[32];
        private List<ICombatTaker> TakerList { get; } = new(32);

        
        public List<ICombatTaker> GetTakers(Vector3 pivot, Vector3 forward, LayerMask layerMask, SizeEntity entity)
        {
            return GetTargetsInRect(pivot, forward, layerMask, entity.Width, entity.Height);
        }
        
        
        private List<ICombatTaker> GetTargetsInRect(Vector3 pivot, Vector3 forward, LayerMask layer, float width, float height)
        {
            Array.Clear(Buffer, 0, Buffer.Length);

            var center = pivot + forward * (height * 0.5f);
            var halfExtents = new Vector3(width    * 0.5f, height * 0.5f, height * 0.5f);

            // Rotate the forward vector 90 degrees around the Y axis for the "right" direction.
            var right = Quaternion.Euler(0, 90, 0) * forward;
            var orientation = Quaternion.LookRotation(forward, Vector3.up);
            var size = Physics.OverlapBoxNonAlloc(center, halfExtents, Buffer, orientation, layer);

            TakerList.Clear();

            for (var i = 0; i < size; i++)
            {
                var collider = Buffer[i];

                if (collider is null || 
                    !collider.TryGetComponent(out ICombatTaker taker) ||
                    !taker.Alive.Value) continue;

                var projectedPosition = Vector3.ProjectOnPlane(collider.transform.position - pivot, Vector3.up);
                var toProjectedPosition = projectedPosition - Vector3.zero;

                if (Vector3.Dot(toProjectedPosition, forward) <= height &&
                    Vector3.Dot(toProjectedPosition, right)   <= width * 0.5f)
                    TakerList.Add(taker);
            }

            return TakerList;
        }
    }
}
