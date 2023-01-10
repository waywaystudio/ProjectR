using System;
using Core;
using UnityEngine;

namespace Character.Combat.Projector
{
    public class SphereProjectorCollider : ProjectorCollider
    {
        [SerializeField] private SphereCollider sphereCollider;

        private const int MaxBuffer = 50;
        private readonly Collider[] buffer = new Collider[MaxBuffer];

        public override void Generate(Vector3 pointA, Vector3 pointB)
        {
           
        }
        
        public override void Initialize(LayerMask targetLayer, float modifier)
        {
            base.Initialize(targetLayer, modifier);
            
            sphereCollider.radius = Modifier;
        }

        public Collider[] DetectCollider()
        {
            var hitCount = Physics.OverlapSphereNonAlloc(transform.position, Modifier, buffer, TargetLayer);

            return hitCount != 0
                ? buffer
                : null;
        }

        public void OnProjectorFinished()
        {
            var colliders = DetectCollider();

            if (colliders.IsNullOrEmpty()) return;
            
            colliders.ForEach(x =>
            {
                if (x.IsNullOrEmpty()) return;
                
                Debug.Log($"Colliding on {x.name} object");
                // Do Damage
                // Do BUff
                // Do StatusEffect...
                // Do Create Floor Effect?
            });            
        }

        public void Test() => OnProjectorFinished();

        public override void OnCollisionEnter(Collision collisionInfo)
        {
            if (collisionInfo.gameObject.IsInLayerMask(TargetLayer) && 
                collisionInfo.gameObject.TryGetComponent(out ICombatTaker taker))
            {
                /* OnCollisionEntered */
                
                // do Call To Character;
                // Try use Method 4: The ConstantPath type
                // Avoid일 수도 있고, Gathering 일 수도 있고...
                // https://arongranberg.com/astar/documentation/dev_4_3_61_b7b7a3f3/wander.html
            }
        }

#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
            
            sphereCollider.radius = Modifier;
        }
#endif
    }
}
