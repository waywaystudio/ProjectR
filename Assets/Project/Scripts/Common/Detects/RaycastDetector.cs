using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Detects
{
    public class RaycastDetector : ITakerDetection
    {
        private RaycastHit[] Buffer { get; } = new RaycastHit[32];
        private List<ICombatTaker> TakerList { get; } = new (32);

        
        public List<ICombatTaker> GetTakers(Vector3 pivot, Vector3 forward, LayerMask layerMask, SizeEntity entity)
        {
            return CastRay(pivot, forward, layerMask, entity.Height);
        }

        public List<ICombatTaker> CastRay(Vector3 pivot, Vector3 forward, LayerMask layer, float distance)
        {
            Array.Clear(Buffer, 0, Buffer.Length);
            
            TakerList.Clear();
        
            var hitCount = Physics.RaycastNonAlloc(pivot, forward, Buffer, distance, layer);
            
            for (var i = 0; i < hitCount; i++)
            {
                if (Buffer[i].collider.TryGetComponent(out ICombatTaker taker) && taker.Alive.Value)
                {
                    TakerList.Add(taker);
                }
            }

            TakerList.Sort(pivot, SortingType.DistanceAscending);

            return TakerList;
        }
    }
}
