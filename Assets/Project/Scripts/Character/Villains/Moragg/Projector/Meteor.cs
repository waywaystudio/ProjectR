using Common;
using Common.Traps;
using UnityEngine;

namespace Character.Villains.Moragg.Projector
{
    public class Meteor : TrapComponent, IProjectorSections
    {
        public Vector2 SizeVector => new (radius * 2f, radius * 2f);
        public float CastWeightTime => ProlongTime;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.AddExecution("MeteorExecution", () =>
                           {
                               if (TryGetTakerInSphere(out var takerList))
                               {
                                   takerList.ForEach(executor.Execute);
                               }
                           })
                           .AddComplete("Execute", Execution);
        }

        public override void Execution() => ExecuteAction.Invoke();
    }
}
