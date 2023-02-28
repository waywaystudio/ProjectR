using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Systems
{
    public class CollidingSystem : MonoBehaviour
    {
        [SerializeField] private int maxBufferCount = 32;
        
        protected Collider[] Buffers;
        
        public bool TryGetTakersInSphere(SkillComponent skill, out List<ICombatTaker> takerList) => (takerList = 
            GetTakersInSphereType(
                skill.Provider.Object.transform.position, 
                skill.Range, 
                skill.Angle, 
                skill.TargetLayer)
            ).HasElement();

        public bool TryGetTakersInSphere(Vector3 center, float radius, float angle, LayerMask layer,
            out List<ICombatTaker> takerList) => (takerList = GetTakersInSphereType(center, radius, angle, layer)).HasElement();
        

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


        private void Awake()
        {
            Buffers = new Collider[maxBufferCount];
        }
    }
}
