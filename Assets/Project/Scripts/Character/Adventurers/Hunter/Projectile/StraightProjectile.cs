using Common;
using Common.Completion;
using Common.Projectiles;
using DG.Tweening;
using UnityEngine;

namespace Adventurers.Hunter.Projectile
{
    public class StraightProjectile : ProjectileComponent
    {
        [SerializeField] private float speed;
        [SerializeField] private Ease easyType = Ease.Linear;
        [SerializeField] private SphereCollider projectileCollider;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private PowerCompletion power;
        
        
        public override void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            power.Initialize(Provider, ActionCode);
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
                power.Damage(taker);
            }
        }
    }
}
