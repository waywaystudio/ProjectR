using UnityEngine;

namespace Common.Projectors
{
    public class ArcProjector : ProjectorComponent
    {
        [SerializeField] private Transform colliderTransform;
        
        protected override void Initialize()
        {
            // sizeVector.x = range
            // sizeVector.y = angle
            var sizeVector = SizeReference.Invoke();
            var range      = sizeVector.x * 2f;

            projector.size               =  new Vector3(range, range, ProjectorDepth);
            colliderTransform.localScale =  new Vector3(sizeVector.x, sizeVector.x, sizeVector.x);
            
            ResetMaterial();
            OffObject();
        }
        
        protected override void OnObject()
        {
            DecalObject.SetActive(true);
            colliderTransform.gameObject.SetActive(true);
        }

        protected override void OffObject()
        {
            DecalObject.SetActive(false);
            colliderTransform.gameObject.SetActive(false);
        }
    }
}
