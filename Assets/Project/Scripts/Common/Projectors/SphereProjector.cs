using UnityEngine;

namespace Common.Projectors
{
    public class SphereProjector : ProjectorComponent
    {
        [SerializeField] private SphereCollider obstacleCollider;
        
        protected override void Initialize()
        {
            var sizeVector = SizeReference.Invoke();

            projector.size          = new Vector3(sizeVector.x, sizeVector.y, ProjectorDepth);
            obstacleCollider.radius = sizeVector.x * 0.5f;
            
            ResetMaterial();
            DecalObject.SetActive(false);
        }
    }
}
