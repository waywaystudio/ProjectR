using Common.Particles;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Skills
{
    public class Slash : SkillComponent
    {
        [SerializeField] private ParticlePlayer slashParticle;
        [SerializeField] private Pool<ParticleComponent> impactParticle;
        
        public override void Initialize()
        {
            base.Initialize();
            
            slashParticle.Initialize(this, transform);
            impactParticle.Initialize(CreateImpactParticle);
            
            cost.PayCondition.Add("HasTarget", detector.HasTarget);
            Builder
                .Add(Section.Execute, "PlaySlashParticle", PlaySlashParticle)
                .Add(Section.Execute, "SlashAttack", SlashAttack);
        }


        private void SlashAttack()
        {
            detector.GetTakers()?.ForEach(taker =>
            {
                impactParticle.Get().Play(taker.Position, taker.gameObject.transform);
                executor.ToTaker(taker);
            });
        }
        
        private void PlaySlashParticle()
        {
            // slashParticle.Play();
            // slashParticle.ParticleRenderer.flip = Provider.Forward.x < 0 
            //     ? Vector3.right 
            //     : Vector3.zero;
        }

        private void CreateImpactParticle(ParticleComponent pc)
        {
            pc.Pool = impactParticle;
        }
    }
}

