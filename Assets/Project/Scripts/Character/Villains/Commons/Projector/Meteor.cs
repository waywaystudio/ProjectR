using Common;
using Common.Particles;
using Common.Traps;
using UnityEngine;

namespace Character.Villains.Commons.Projector
{
    public class Meteor : TrapComponent, IProjectionProvider
    {
        [SerializeField] private SinglePool<ParticleComponent> explodeParticle;
        
        public Vector3 SizeVector => new (radius, radius, 360f);
        public float CastingWeight => ProlongTime;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            explodeParticle.Initialize();

            SequenceBuilder.Add(Section.Execute,"MeteorExecution", () =>
                           {
                               if (TryGetTakerInSphere(out var takerList))
                               {
                                   takerList.ForEach(executor.ToTaker);
                               }
                           })
                           .Add(Section.Complete,"Execute", Execution)
                           .Add(Section.Complete, "PlayExplodeParticle", PlayExplodeParticle);
        }

        public override void Execution() => SequenceInvoker.Execute();


        private void PlayExplodeParticle()
        {
            explodeParticle.Get().Play(transform.position, null);
        }
    }
}
