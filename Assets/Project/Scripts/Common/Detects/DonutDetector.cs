using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Detects
{
    public class DonutDetector : ITakerDetection
    {
        private Collider[] outerBuffer { get; } = new Collider[32];
        private Collider[] innerBuffer { get; } = new Collider[32];
        private List<ICombatTaker> TakerList { get; } = new(32);
        
       
        public List<ICombatTaker> GetTakers(Vector3 pivot, Vector3 forward, LayerMask layerMask, SizeEntity entity)
        {
            var outerRadius = entity.OuterRadius;
            var innerRadius = entity.InnerRadius;

            // Get all targets within the larger sphere.
            var outerCount = Physics.OverlapSphereNonAlloc(pivot, outerRadius, outerBuffer, layerMask);
            if (outerCount == 0) return null;

            // Get all targets within the smaller sphere.
            var innerCount = Physics.OverlapSphereNonAlloc(pivot, innerRadius, innerBuffer, layerMask);

            // Reset the taker list.
            TakerList.Clear();

            // Go through all targets in the outer sphere.
            for (var i = 0; i < outerCount; i++)
            {
                var outerCollider = outerBuffer[i];

                // Skip null colliders.
                if (outerCollider is null) continue;

                // If this target is also in the inner sphere, skip it.
                if (Array.IndexOf(innerBuffer, outerCollider, 0, innerCount) >= 0) continue;

                // At this point, the target is in the outer sphere but not the inner sphere.
                // If it's an ICombatTaker, add it to the list.
                if (outerCollider.TryGetComponent(out ICombatTaker taker) && taker.Alive.Value)
                {
                    TakerList.Add(taker);
                }
            }

            return TakerList;
        }
    }
}
