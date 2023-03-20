using UnityEngine;

namespace Common.Projectors
{
    public class SphereProjector : ProjectorComponent
    {
        [SerializeField] private SphereCollider sphereCollider;
        
        // CastingTime은 FloatEvent를 쓸 수 있을 것 같다.
        // Radius는 Func<float>으로 받을 수도 있다.
        // Width, Height를 받아야 하기도 한다.

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
