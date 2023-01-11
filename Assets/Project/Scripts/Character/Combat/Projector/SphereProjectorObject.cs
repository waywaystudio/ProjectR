using DG.Tweening;
using UnityEngine;

namespace Character.Combat.Projector
{
    public class SphereProjectorObject : ProjectorObject
    {
        [SerializeField] private SphereCollider sphereCollider;

        protected override Collider ProjectorCollider
        {
            get => sphereCollider;
            set => sphereCollider = value as SphereCollider;
        }

        protected override void SetTransform()
        {
            transform.position = Taker.Object.transform.position;
        }

        protected override void OnGenerated()
        {
            var decalLength = sizeValue * 2f;
            
            sphereCollider.enabled = true;
            sphereCollider.radius  = sizeValue;
            
            projectorDecal.size            = new Vector3(decalLength, decalLength, 50f);
            decalMaterial
                .DOFloat(1.5f, FillAmount, CastingTime)
                .OnComplete(OnFinished.Invoke);
        }
    }
}
