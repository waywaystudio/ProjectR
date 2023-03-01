using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Systems
{
    public class CollidingSystem : MonoBehaviour
    {
        [SerializeField] private int maxBufferCount = 32;
        
        protected Collider[] ColliderBuffers;
        protected RaycastHit[] RayBuffers;
        
        public bool TryGetTakersInSphere(SkillComponent skill, out List<ICombatTaker> takerList) => (takerList = 
            GetTakersInSphereType(
                skill.Provider.Object.transform.position, 
                skill.Range, 
                skill.Angle, 
                skill.TargetLayer)
            ).HasElement();

        public bool TryGetTakersInSphere(Vector3 center, float radius, float angle, LayerMask layer,
            out List<ICombatTaker> takerList) => (takerList = GetTakersInSphereType(center, radius, angle, layer)).HasElement();

        public bool TryGetTakersByRaycast(Vector3 center, Vector3 target, float distance, int maxCount,
                                          LayerMask targetLayer,
                                          out List<ICombatTaker> takerList) =>
            (takerList = GetTakerByRaycast(center, target, distance, maxCount, targetLayer)).HasElement();

        public bool TryGetTakerByRayCast(Vector3 center, Vector3 target, float distance, int maxCount,
                                         LayerMask targetLayer, out ICombatTaker taker) =>
            (taker = GetTakerByRaycast(center, target, distance, maxCount, targetLayer).FirstOrNull()) != null;


        private List<ICombatTaker> GetTakersInSphereType(Vector3 center, float radius, float angle, LayerMask layer)
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
        
        public List<ICombatTaker> GetTakerByRaycast(Vector3 center, Vector3 target, float distance, float maxCount, LayerMask targetLayer)
        {
            var direction = (target - center).normalized;
            
            if (Physics.RaycastNonAlloc(center, direction, RayBuffers, distance) == 0)
            {
                Debug.Log("Noting In Raycast");
            }

            // TODO. 가까운 거리 기준으로 먼저 들어오나 확인해야 함.
            if (Physics.RaycastNonAlloc(center, direction, RayBuffers, distance, targetLayer) == 0)
            {
                return null;
            }
            
            Debug.DrawRay(center, direction, Color.green, 2, false);
            
            var result  = new List<ICombatTaker>();
            var counter = 0;

            foreach (var ray in RayBuffers)
            {
                if (ray.collider.IsNullOrEmpty() || !ray.collider.TryGetComponent(out ICombatTaker taker)) continue;
                if (counter > maxCount)
                {
                    return result;
                }
                
                result.Add(taker);
                counter++;
            }

            return null;
        }

        private List<ICombatTaker> GetTakersInRectangleType(Vector3 center, float width, float height, LayerMask layer)
        {
            return null;
        }


        private void Awake()
        {
            ColliderBuffers = new Collider[maxBufferCount];
            RayBuffers      = new RaycastHit[maxBufferCount];
        }
    }
}
