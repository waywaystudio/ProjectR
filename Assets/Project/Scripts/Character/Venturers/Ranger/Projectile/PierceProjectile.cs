using Common;
using Common.Particles;
using Common.Projectiles;
using UnityEngine;

namespace Character.Venturers.Ranger.Projectile
{
    [RequireComponent(typeof(SphereCollider))]
    public class PierceProjectile : ProjectileComponent
    {
        [SerializeField] private SinglePool<ParticleInstance> flyingParticle;
        [SerializeField] private Pool<ParticleInstance> hitParticle;
        [SerializeField] private SphereCollider triggerCollider;
        [SerializeField] private int maxPierceCount = 1;

        private int pierceCount;

        public ICombatTaker PiercedTaker { get; private set; }

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            flyingParticle.Initialize(null, transform);
            hitParticle.Initialize(component => component.Pool = hitParticle);
            
            Builder
                .Add(Section.Active, "CollidingTriggerOn", () => triggerCollider.enabled = true)
                .Add(Section.Active, "ResetPierceCount", () => pierceCount = 0)
                .Add(Section.Active, "PlayFlyingParticle", PlayFlyingParticle)
                .Add(Section.Execute, "PierceProjectileExecution", PierceProjectileExecution)
                .Add(Section.End, "CollidingTriggerOff", () => triggerCollider.enabled = false)
                .Add(Section.End, "StopFlyingParticle", StopFlyingParticle);
        }

        public void PierceProjectileExecution()
        {
            if (PiercedTaker is not null && pierceCount <= maxPierceCount)
            {
                executor.ToTaker(PiercedTaker);
                executor.ToPosition(PiercedTaker.Position);
                
                var particle = hitParticle.Get();
                
                particle.Play(PiercedTaker.Position, PiercedTaker.gameObject.transform);

                if (++pierceCount > maxPierceCount)
                {
                    Invoker.Complete();
                }
            }
        }


        private void PlayFlyingParticle() => flyingParticle.Get().Play();
        private void StopFlyingParticle() => flyingParticle.Release();

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
