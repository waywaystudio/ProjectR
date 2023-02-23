using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Targeting
{
    public class Colliding : MonoBehaviour
    {
        protected readonly Collider[] Buffers = new Collider[32];

        public bool TryGetTakersInSphere(Vector3 center, float radius, float angle, LayerMask layer,
            out List<ICombatTaker> takerList)
        {
            return (takerList = GetTakersInSphereType(center, radius, angle, layer)).HasElement();
        }
        

        private void ShowProjector(float radius /*Texture or Shader? decal*/) { }

        private List<ICombatTaker> GetTakersInSphereType(Vector3 center, float radius, float angle, LayerMask layer)
        {
            if (Physics.OverlapSphereNonAlloc(center, radius, Buffers, layer) == 0) return null;

            var result = new List<ICombatTaker>();
            
            Buffers.ForEach(collider =>
            {
                if (collider.IsNullOrEmpty() || !collider.TryGetComponent(out ICombatTaker taker)) return;
                
                if (Mathf.Abs(angle - 360.0f) > 0.000001f)
                {
                    var direction = (collider.transform.position - center).normalized;

                    if (Vector3.Angle(transform.forward, direction) > angle * 0.5f) return;
                }
                
                result.Add(taker);
            });

            return result;
        }

        private List<ICombatTaker> GetTakersInRectangleType(Vector3 center, float width, float height, LayerMask layer)
        {
            return null;
        }
    }
}
