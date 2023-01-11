using Core;
using UnityEngine;

namespace Character.Combat.Projector.Colliders
{
    public class SphereProjectorCollider : ProjectorCollider
    {
        [SerializeField] private SphereCollider sphereCollider;

        private const int MaxBuffer = 50;
        private readonly Collider[] buffer = new Collider[MaxBuffer];

        protected override void Generate(Vector3 providerPosition, Vector3 targetPosition)
        {
            sphereCollider.enabled = true;
        }
        
        protected override void OnFinish()
        {
            sphereCollider.enabled = false;
        }

        // public override void OnProjectorFinished()
        // {
        //     // Variant
        //     Physics.OverlapSphereNonAlloc(transform.position, sizeValue, buffer, targetLayer);
        //     //
        //
        //     if (buffer.IsNullOrEmpty()) return;
        //     
        //     buffer.ForEach(x =>
        //     {
        //         if (x.IsNullOrEmpty()) return;
        //         
        //         Debug.Log($"Colliding on {x.name} object");
        //         // Do FinishAction
        //     });   
        //     
        //     sphereCollider.enabled = false;
        // }

        protected override void Awake()
        {
            base.Awake();
            
            sphereCollider.radius  = sizeValue;
            sphereCollider.enabled = false;
        }

#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
            
            sphereCollider.radius  = sizeValue;
            sphereCollider.enabled = false;
        }
#endif
    }
}
