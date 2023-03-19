using Common;
using Common.Execution;
using Common.Projectiles;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Adventurers.Hunter.Projectile
{
    public class StraightProjectile : ProjectileComponent
    {
        [SerializeField] private float speed;
        [SerializeField] private Ease easyType = Ease.Linear;
        [SerializeField] private SphereCollider projectileCollider;
        [SerializeField] private LayerMask targetLayer;
        [FormerlySerializedAs("power")] [SerializeField] private StatusEffectDamageExecution damage;
        
        
        public override void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            // damage.Initialize(Provider);
            // damage.Initialize(Provider, ActionCode);
        }
        
        protected void Flying(Vector3 destination)
        {
            if (speed == 0f)
            {
                Debug.LogWarning("Projectile Speed Value Can not 0f.");
                return;
            }
            
            transform.DOMove(destination, 1f / speed)
                     .SetEase(easyType);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.IsInLayerMask(targetLayer) && other.TryGetComponent(out ICombatTaker taker))
            {
                damage.Execution(taker);
            }
        }
    }
}
