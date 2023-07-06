using Character.Venturers.Ranger.Projectile;
using Common.Particles;
using UnityEngine;

namespace Character.Venturers.Ranger.Fx
{
    public class InstantShotProjectileFx : MonoBehaviour, IEditable
    {
        [SerializeField] private PierceProjectile projectile;
        [SerializeField] private GameObject flyingParticlePrefab;
        [SerializeField] private GameObject hitParticlePrefab;

        [SerializeField] private Pool<ParticleComponent> hitParticlePool;

        private ParticleSystem flyingParticle;

        private void PlayFlyingParticle()
        {
            flyingParticle.gameObject.SetActive(true);
            flyingParticle.Play(true);
        }

        private void StopFlyingParticle()
        {
            flyingParticle.gameObject.SetActive(false);
            flyingParticle.Stop(true);
        }
        
        private void Awake()
        {
            if (!flyingParticlePrefab.ValidInstantiate(out flyingParticle, transform)) return;

            var builder = new SequenceBuilder(projectile.Sequencer);
        
            builder
                .Add(SectionType.Active, "PlayFlyingVfx", PlayFlyingParticle)
                .Add(SectionType.Execute, "PlayHitParticle", PlayHitParticle)
                .Add(SectionType.End, "StopFlyingVfx", StopFlyingParticle);
        }
        
        
        
        private void CreateHitParticle(ParticleComponent particle)
        {
            var taker = projectile.PiercedTaker;
            // statusEffect.Initialize(Origin.Provider);

            var builder = new SequenceBuilder(particle.Sequencer);

            // builder.Add(SectionType.End, "ReturnTransform", () => statusEffect.transform.SetParent(transform, false))
            //        .Add(SectionType.End, "ReleasePool", () => pool.Release(statusEffect));
        }
        
        private void PlayHitParticle()
        {
            if (!hitParticlePrefab.ValidInstantiate(out ParticleSystem hit, null)) return;

            hit.transform.position = projectile.PiercedTaker.Position + Vector3.up * 3f;
            hit.gameObject.SetActive(true);
            hit.Play(true);
        }
        
        private void OnEnable()
        {
            hitParticlePool.Initialize(CreateHitParticle);
        }

        private void OnDisable()
        {
            hitParticlePool.Clear();
        }
        
        private void PlayHitParticleSim()
        {
            // if (!hitParticlePrefab.ValidInstantiate(out ParticleComponent hit, null)) return;
            var particleComponent = hitParticlePool.Get();
            var targetPosition = projectile.PiercedTaker.Position + Vector3.up * 3f;
            
            particleComponent.Play(targetPosition);

            // hit.Play();
            // hit.transform.position = projectile.PiercedTaker.Position + Vector3.up * 3f;
            // hit.gameObject.SetActive(true);
            // hit.Play(true);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            projectile = GetComponentInParent<PierceProjectile>();

            if (!Verify.IsNotNull(projectile, "Missing Projectile Class in InstantShotProjectileFx")) 
                return;
        }
#endif
    }
}
