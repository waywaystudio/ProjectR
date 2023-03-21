using UnityEngine;

namespace Common.Projectiles
{
    [RequireComponent(typeof(SphereCollider))]
    public class ProjectileCollidingTrigger : MonoBehaviour
    {
        [SerializeField] protected SphereCollider triggerCollider;
        
        private LayerMask targetLayer;
        private ProjectileComponent projectileComponent;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICombatTaker _) && other.gameObject.IsInLayerMask(targetLayer))
            {
                Debug.Log(other.name);
                
                projectileComponent.Complete();
            }
        }
        
        private void Awake()
        {
            TryGetComponent(out projectileComponent);

            targetLayer            =   projectileComponent.TargetLayer;
            triggerCollider        ??= GetComponent<SphereCollider>();
            
            projectileComponent.OnActivated.Register("CollidingTriggerOn", () => triggerCollider.enabled = true);
            projectileComponent.OnEnded.Register("CollidingTriggerOff", () => triggerCollider.enabled    = false);
        }
    }
}
