using Core;
using UnityEngine;

namespace Character.Combat.Projector.Colliders
{
    public class CircleProjectorCollider : ProjectorCollider
    {
        [SerializeField] private SphereCollider sphereCollider;

        private float radius;

        protected override void StartProjection()
        {
            TargetLayer           = po.TargetLayer;
            po.transform.position = po.Taker != null
                ? po.Taker.Object.transform.position
                : po.Destination;
        }

        protected override void EndProjection()
        {
            Physics.OverlapSphereNonAlloc(transform.position, radius, ColliderBuffer, TargetLayer);

            if (ColliderBuffer.IsNullOrEmpty()) return;
            
            ColliderBuffer.ForEach(x =>
            {
                if (x.IsNullOrEmpty()) return;
                if (!x.TryGetComponent(out ICombatTaker taker)) return;
                
                po.OnProjectorEnd.Invoke(taker);
            });
       }

        protected override void Awake()
        {
            base.Awake();

            radius                =   SizeValue.x;
            sphereCollider        ??= GetComponent<SphereCollider>();
            sphereCollider.radius =   radius;
        }

#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
            
            sphereCollider ??= GetComponent<SphereCollider>();
        }
#endif
    }
}
