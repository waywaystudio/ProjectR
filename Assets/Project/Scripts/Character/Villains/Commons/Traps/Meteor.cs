using Common;
using Common.Projectors;
using Common.Traps;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Villains.Commons.Traps
{
    public class Meteor : Trap, IProjection
    {
        [FormerlySerializedAs("projector")] [SerializeField] private Projection projection;
        
        private readonly Collider[] colliderBuffers = new Collider[32];
        
        public float CastingTime => ProlongTime;
        
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            projection.Initialize(this);
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
