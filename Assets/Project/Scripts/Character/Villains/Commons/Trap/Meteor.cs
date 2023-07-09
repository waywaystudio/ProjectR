using Common;
using UnityEngine;

namespace Character.Villains.Commons.Trap
{
    public class Meteor : Common.Traps.Trap, IProjectionProvider
    {
        private readonly Collider[] colliderBuffers = new Collider[32];
        
        public float CastingWeight => ProlongTime;
        
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(Section.Execute, "MeteorExecution", MeteorExecution);
        }


        private void MeteorExecution()
        {
            var takerList = TargetUtility.GetTargetsInSphere<ICombatTaker>(transform.position, targetLayer, Radius, colliderBuffers);

            takerList.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
    }
}
