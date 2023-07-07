using Character.Venturers.Ranger.Projectile;
using Common;
using Common.Particles;
using UnityEngine;

namespace Character.Venturers.Ranger.Fx
{
    public class InstantShotProjectileFx : FxBehaviour<PierceProjectile>
    {
        [SerializeField] private GameObject hitParticlePrefab;
        [SerializeField] private ParticlePlayer flyingParticle;
        

        private void PlayHitParticle()
        {
            if (!hitParticlePrefab.ValidInstantiate(out ParticleSystem hit, null)) return;
            
            hit.transform.position = master.PiercedTaker.Position + Vector3.up * 3f;
            hit.gameObject.SetActive(true);
            hit.Play(true);
        }

        private void Awake()
        {
            flyingParticle.Initialize(master, transform);

            var builder = new SequenceBuilder(master.Sequencer);

            builder
                .Add(flyingParticle.PlaySection, "PlayFlyingVfx", flyingParticle.Play)
                .Add(flyingParticle.StopSection, "StopFlyingVfx", flyingParticle.Stop)
                
                // TEMP
                .Add(Section.Execute, "PlayHitParticle", PlayHitParticle);
        }
    }
}
