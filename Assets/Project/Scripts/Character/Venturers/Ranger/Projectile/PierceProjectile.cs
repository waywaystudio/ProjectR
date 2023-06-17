using Common;
using Common.Projectiles;
using UnityEngine;

namespace Character.Venturers.Ranger.Projectile
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

            sequencer.ActiveAction.Add("CollidingTriggerOn", () => triggerCollider.enabled = true);
            sequencer.ActiveAction.Add("ResetPierceCount", () => pierceCount               = 0);
            sequencer.EndAction.Add("CollidingTriggerOff", () => triggerCollider.enabled = false);
        }

        public override void Execution()
        {
            if (piercedTaker is not null && pierceCount <= maxPierceCount)
            {
                executor.Execute(piercedTaker);

                if (++pierceCount > maxPierceCount)
                {
                    sequencer.Complete();
                }
            }
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
