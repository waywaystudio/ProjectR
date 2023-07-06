using Character.Venturers.Ranger.Projectile;
using Common.Particles;
using UnityEngine;

namespace Character.Venturers.Ranger.Fx
{
    public class InstantShotProjectileFx : MonoBehaviour, IEditable
    {
        [SerializeField] private PierceProjectile projectile;
        
        [SerializeField] private GameObject hitParticlePrefab;
        [SerializeField] private ParticlePool flyingParticle;


        private void Awake()
        {
            flyingParticle.Initialize(CreateFlyingParticle, transform);

            var builder = new SequenceBuilder(projectile.Sequencer);

            builder
                .Add(SectionType.Active, "PlayFlyingVfx", flyingParticle.Play)
                .Add(SectionType.End, "StopFlyingVfx", flyingParticle.Stop)
                
                // TEMP
                .Add(SectionType.Execute, "PlayHitParticle", PlayHitParticle);
        }

        private void CreateFlyingParticle(ParticleComponent particle)
        {
            particle.Pool = flyingParticle;
        }
        
        private void PlayHitParticle()
        {
            if (!hitParticlePrefab.ValidInstantiate(out ParticleSystem hit, null)) return;
            
            hit.transform.position = projectile.PiercedTaker.Position + Vector3.up * 3f;
            hit.gameObject.SetActive(true);
            hit.Play(true);
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
