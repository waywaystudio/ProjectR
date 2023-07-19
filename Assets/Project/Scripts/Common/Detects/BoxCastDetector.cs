using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Detects
{
    public class BoxCastDetector : ITakerDetection
    {
        private RaycastHit[] Buffer { get; } = new RaycastHit[32];
        private List<ICombatTaker> TakerList { get; } = new(32);

        
        public List<ICombatTaker> GetTakers(Vector3 pivot, Vector3 forward, LayerMask layerMask, SizeEntity entity)
        {
            return GetTargetsInRect(pivot, forward, layerMask, entity.Width, entity.Height);
        }
        
        
        private List<ICombatTaker> GetTargetsInRect(Vector3 pivot, Vector3 forward, LayerMask layer, float width, float height)
        {
            Array.Clear(Buffer, 0, Buffer.Length);

            // Create a halfExtents vector with half of width and height
            var halfExtents = new Vector3(width / 2f, height / 2f, 0f);
            var orientation = Quaternion.LookRotation(forward);
            var hitCount = Physics.BoxCastNonAlloc(pivot, halfExtents, forward, Buffer, orientation, Mathf.Infinity, layer);

            if (hitCount == 0) return null;

            TakerList.Clear();

            for (var i = 0; i < hitCount; i++)
            {
                var hit = Buffer[i];
                if (hit.collider.TryGetComponent(out ICombatTaker taker) && taker.Alive.Value)
                {
                    TakerList.Add(taker);
                }
            }

            TakerList.Sort(pivot, SortingType.DistanceAscending);

            return TakerList;
        }
    }
}
