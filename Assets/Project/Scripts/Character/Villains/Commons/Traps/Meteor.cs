using Common;
using Common.Projectors;
using Common.Traps;
using UnityEngine;

namespace Character.Villains.Commons.Traps
{
    public class Meteor : Trap, IProjection
    {
        [SerializeField] private ArcProjector projector;
        
        private readonly Collider[] colliderBuffers = new Collider[32];
        
        public float CastingTime => ProlongTime;
        
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            projector.Initialize(this);
            Builder
                .Add(Section.Complete, "MeteorExecution", MeteorExecution)
                .Add(Section.Active, "RotateToIdentity", () => transform.rotation = Quaternion.identity)
                ;
        }


        private void MeteorExecution()
        {
            var takerList = TargetUtility.GetTargetsInSphere<ICombatTaker>(transform.position, targetLayer, Radius, colliderBuffers);

            takerList?.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
    }
}
