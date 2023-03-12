using System.Collections.Generic;
using UnityEngine;

namespace Common.Systems
{
    public class CollidingSystem : MonoBehaviour
    {
        [SerializeField] private int maxBufferCount = 32;
        
        protected Collider[] ColliderBuffers;
        protected RaycastHit[] RayBuffers;


        public bool TryGetTakersInSphere(Vector3 center, float radius, float angle, LayerMask layer,
                                         out List<ICombatTaker> takerList) => (takerList = GetTakersInSphereType(center, radius, angle, layer)).HasElement();

        public bool TryGetTakersByRaycast(Vector3 center, Vector3 direction, float distance, int maxCount,
                                          LayerMask targetLayer,
                                          out List<ICombatTaker> takerList) =>
            (takerList = GetTakerByRaycast(center, direction, distance, maxCount, targetLayer)).HasElement();

        public bool TryGetTakerByRayCast(Vector3 center, Vector3 direction, float distance, int maxCount,
                                         LayerMask targetLayer, out ICombatTaker taker) =>
            (taker = GetTakerByRaycast(center, direction, distance, maxCount, targetLayer).FirstOrNull()) != null;


        public List<ICombatTaker> GetTakersInSphereType(Vector3 center, float radius, float angle, LayerMask layer)
        {
            if (Physics.OverlapSphereNonAlloc(center, radius, ColliderBuffers, layer) == 0) return null;

            var result = new List<ICombatTaker>();
            
            ColliderBuffers.ForEach(collider =>
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
        
        private List<ICombatTaker> GetTakerByRaycast(Vector3 center, Vector3 direction, float distance, float maxCount, LayerMask targetLayer)
        {
            // Add order : Distance Descendent
            if (Physics.RaycastNonAlloc(center, direction, RayBuffers, distance, targetLayer.value) == 0) return null;

            var result  = new List<ICombatTaker>();
            var counter = 0;

            // Reverse For loop for check nearest collider.
            for (var i = RayBuffers.Length - 1; i >= 0; i--)
            {
                if (RayBuffers[i].collider.IsNullOrEmpty() || !RayBuffers[i].collider.TryGetComponent(out ICombatTaker taker)) continue;
                if (counter > maxCount) break;

                result.Add(taker);
                counter++;
            }
            
            return result;
        }

        // private List<ICombatTaker> GetTakersInRectangleType(Vector3 center, float width, float height, LayerMask layer)
        // {
        //     return null;
        // }


        private void Awake()
        {
            ColliderBuffers = new Collider[maxBufferCount];
            RayBuffers      = new RaycastHit[maxBufferCount];
        }
    }
}
