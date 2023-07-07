using Common.Particles;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Skills
{
    public class Slash : SkillComponent
    {
        // [SerializeField] private SinglePool<ParticleComponent> slashParticle;
        // [SerializeField] private Pool<ParticleComponent> impactParticle;
        
        public override void Initialize()
        {
            base.Initialize();
            
            // slashParticle.Initialize(null, transform);
            // impactParticle.Initialize(component => component.Pool = impactParticle);
            
            cost.PayCondition.Add("HasTarget", detector.HasTarget);
            Builder
                .Add(Section.Execute, "PlaySlashParticle", PlaySlashParticle)
                .Add(Section.Execute, "SlashAttack", SlashAttack);
        }


        private void SlashAttack()
        {
            detector.GetTakers()?.ForEach(taker =>
            {
                Taker = taker;
                
                // var particle = impactParticle.Get();
                // particle.Play(taker.Position, taker.gameObject.transform);
                
                Invoker.ExtraAction();
                executor.ToTaker(taker);
            });
        }
        
        private void PlaySlashParticle()
        {
            // slashParticle.Get().Play();
        }
    }
}

