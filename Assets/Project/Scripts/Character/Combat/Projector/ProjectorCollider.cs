using System;
using UnityEngine;

namespace Character.Combat.Projector
{
    public abstract class ProjectorCollider : MonoBehaviour
    {
        protected LayerMask TargetLayer;
        protected float Modifier;
        
        public Collider Collider { get; set; }

        public abstract void Generate(Vector3 pointA, Vector3 pointB);
        public abstract void OnCollisionEnter(Collision collisionInfo);

        public virtual void Initialize(LayerMask targetLayer, float modifier)
        {
            TargetLayer = targetLayer;
            Modifier    = modifier;
        }
        

#if UNITY_EDITOR
        public virtual void SetUp()
        {
            
        }
#endif
    }
}
