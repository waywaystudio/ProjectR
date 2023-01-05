using Core;
using DG.Tweening;
using UnityEngine;

namespace Character.Combat.Projectile
{
    public class HealOrbProjectile : ProjectileBehaviour
    {
        [SerializeField] private float orbOffset = 3.5f;
        [SerializeField] private LayerMask allyLayer;

        private Vector3 Offset
        {
            get
            {
                if (ValidateTaker)
                {
                    var takerPosition = Taker.Object.transform.position;
                    var direction = (takerPosition - transform.position).normalized;
                    var offset = direction * orbOffset;
                    
                    return takerPosition + offset;
                }

                return Vector3.zero;
            }
        }

        protected override void Trajectory()
        {
            TrajectoryTweener = transform
                .DOMove(Offset, speed)
                .SetEase(Ease.OutCubic)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(Arrived)
                .SetSpeedBased();
        }


        protected void Arrived()
        {
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.IsInLayerMask(allyLayer)) Heal(other);
            if (other.gameObject.IsInLayerMask(TargetLayer)) Damage(other);
        }

        private void Heal(Component other)
        {
            if (ValidateTaker && other.TryGetComponent(out ICombatTaker ally))
            {
                ally.TakeHeal(HealEntity);
            }
        }

        private void Damage(Component other)
        {
            if (ValidateTaker && other.TryGetComponent(out ICombatTaker enemy))
            {
                enemy.TakeDamage(DamageEntity);
            }
        }
    }
}
