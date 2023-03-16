using Pathfinding;
using UnityEngine;

namespace Common.Projectors
{
    public class SphereProjector : ProjectorComponent, IPoolable<SphereProjector>
    {
        [SerializeField] private DynamicGridObstacle dynamicObstacle;
        
        // # float CastingTime;
        private ICombatTaker taker;
        private Vector3 pivot = Vector3.negativeInfinity;
        private SphereCollider sphereCollider;
        private SphereCollider SphereCollider => sphereCollider ??= GetComponentInChildren<SphereCollider>();

        public Pool<SphereProjector> Pool { get; set; }

        public void Initialize(float castingTime, float radius)
        {
            if (IsFirst)
            {
                InitialSetUp();
                
                sphereCollider = GetComponentInChildren<SphereCollider>();
            }

            CastingTime           = castingTime;
            projector.size        = new Vector3(radius * 2f, radius * 2f, ProjectorDepth);
            SphereCollider.radius = radius;
        }

        public void SetTaker(ICombatTaker taker) => this.taker = taker;
        public void SetPosition(Vector3 position) => transform.position = position;


        private void Update()
        {
            if (taker is null) return;
            
            transform.position = taker.gameObject.transform.position;
        }

        protected void OnValidate()
        {
            sphereCollider = GetComponentInChildren<SphereCollider>();
            
            if (!sphereCollider.gameObject.IsInLayerMask(LayerMask.GetMask("Projector")))
            {
                Debug.LogError($"Projector layer must be Projector. Input:{LayerMask.LayerToName(sphereCollider.gameObject.layer)}");
            }
        }
    }
}
