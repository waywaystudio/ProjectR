using DG.Tweening;
using UnityEngine;

namespace Character.Combat.Projector
{
    public class RectangleProjectorObject : ProjectorObject
    {
        [SerializeField] private BoxCollider boxCollider;
        
        protected override Collider ProjectorCollider
        {
            get => boxCollider;
            set => boxCollider = value as BoxCollider;
        }

        private const float BoxHeight = 2f;
        
        protected override void SetTransform()
        {
            var takerPosition = Taker.Object.transform.position;
            var providerPosition = Provider.Object.transform.position;

            transform.position = (takerPosition + providerPosition) * 0.5f;
            transform.LookAt(takerPosition);
        }

        protected override void OnGenerated()
        {
            var takerPosition = Taker.Object.transform.position;
            var providerPosition = Provider.Object.transform.position;

            var width = sizeValue;
            var length = Vector3.Distance(providerPosition, takerPosition);
            
            boxCollider.enabled = true;
            boxCollider.size    = new Vector3(width, BoxHeight, length);
            
            projectorDecal.size = new Vector3(width, length, 50f);
            decalMaterial
                .DOFloat(1.0f, FillAmount, CastingTime)
                .OnComplete(OnFinished.Invoke);
        }
    }
}
