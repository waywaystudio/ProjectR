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

        public ICombatTaker PiercedTaker { get; private set; }

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            Builder
                .Add(SectionType.Active, "CollidingTriggerOn", () => triggerCollider.enabled = true)
                .Add(SectionType.Active, "ResetPierceCount", () => pierceCount = 0)
                .Add(SectionType.Execute, "PierceProjectileExecution", PierceProjectileExecution)
                .Add(SectionType.End, "CollidingTriggerOff", () => triggerCollider.enabled = false);
        }

        public void PierceProjectileExecution()
        {
            if (PiercedTaker is not null && pierceCount <= maxPierceCount)
            {
                executor.ToTaker(PiercedTaker);
                executor.ToPosition(PiercedTaker.Position);

                if (++pierceCount > maxPierceCount)
                {
                    Invoker.Complete();
                }
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICombatTaker taker) && other.gameObject.IsInLayerMask(targetLayer))
            {
                PiercedTaker = taker;
                Invoker.Execute();
            }
        }
    }
}
