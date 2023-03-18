using UnityEngine;

namespace Common.Projectors
{
    public class SphereProjector : ProjectorComponent
    {
        [SerializeField] private SphereCollider sphereCollider;

        public void Initialize(float castingTime, float radius, ISequence mainSequence)
        {
            CastingTime           = castingTime;
            projector.size        = new Vector3(radius * 2f, radius * 2f, ProjectorDepth);
            sphereCollider.radius = radius;
            
            CoreInitialize(mainSequence);
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
