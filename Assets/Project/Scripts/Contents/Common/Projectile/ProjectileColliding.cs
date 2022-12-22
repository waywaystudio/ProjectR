using Core;
using UnityEngine;

namespace Common.Projectile
{
    public class ProjectileColliding : MonoBehaviour
    {
        private ProjectileBehaviour pb;

        public ProjectileBehaviour Pb => pb ??= GetComponentInParent<ProjectileBehaviour>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.IsInLayerMask(Pb.TargetLayer))
            {
                Pb.OnCollided?.Invoke();
            }
        }
    }
}
