using Common;
using Common.Projectiles;
using UnityEngine;

namespace Character.Venturers.Hunter.Projectile
{
    [RequireComponent(typeof(SphereCollider))]
    public class PierceProjectile : ProjectileComponent
    {
        [SerializeField] private SphereCollider triggerCollider;
        [SerializeField] private int maxPierceCount = 1;

        private int pierceCount;

        private ICombatTaker piercedTaker;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            OnActivated.Register("CollidingTriggerOn", () => triggerCollider.enabled = true);
            OnActivated.Register("ResetPierceCount", () => pierceCount = 0);
            OnEnded.Register("CollidingTriggerOff", () => triggerCollider.enabled    = false);
        }

        public override void Execution()
        {
            if (piercedTaker is not null && pierceCount <= maxPierceCount)
            {
                ExecutionTable.Execute(piercedTaker);

                if (++pierceCount > maxPierceCount)
                {
                    Complete();
                }
            }
        }

        public override void Complete()
        {
            OnCompleted.Invoke();

            End();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICombatTaker taker) && other.gameObject.IsInLayerMask(targetLayer))
            {
                piercedTaker = taker;
                Execution();
            }
        }
    }
}
