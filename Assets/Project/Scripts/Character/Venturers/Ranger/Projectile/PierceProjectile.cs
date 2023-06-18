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
            
            SequenceBuilder.Add(SectionType.Active, "CollidingTriggerOn", () => triggerCollider.enabled = true)
                           .Add(SectionType.Active, "ResetPierceCount", () => pierceCount = 0)
                           .Add(SectionType.End, "CollidingTriggerOff", () => triggerCollider.enabled = false);
        }

        public override void Execution()
        {
            if (piercedTaker is not null && pierceCount <= maxPierceCount)
            {
                executor.Execute(piercedTaker);

                if (++pierceCount > maxPierceCount)
                {
                    SequenceInvoker.Complete();
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
