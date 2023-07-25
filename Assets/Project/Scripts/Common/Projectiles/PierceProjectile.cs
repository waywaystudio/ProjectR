using UnityEngine;

namespace Common.Projectiles
{
    [RequireComponent(typeof(SphereCollider))]
    public class PierceProjectile : Projectile
    {
        [SerializeField] private SphereCollider triggerCollider;
        [SerializeField] private int maxPierceCount = 1;

        private int pierceCount;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            Builder
                .Add(Section.Active, "CollidingTriggerOn", () => triggerCollider.enabled = true)
                .Add(Section.Active, "ResetPierceCount", () => pierceCount = 0)
                .Add(Section.Execute, "PierceProjectileExecution", PierceProjectileExecution)
                .Add(Section.End, "CollidingTriggerOff", () => triggerCollider.enabled = false)
                ;
        }

        public void PierceProjectileExecution()
        {
            if (Taker is not null && pierceCount <= maxPierceCount)
            {
                Invoker.Hit(Taker);
                Invoker.Fire(Taker.Position);

                if (++pierceCount > maxPierceCount)
                {
                    Invoker.Complete();
                }
            }
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICombatTaker taker) && 
                other.gameObject.IsInLayerMask(targetLayer) &&
                taker.Alive.Value)
            {
                Taker = taker;
                Invoker.Execute();
            }
        }
    }
}