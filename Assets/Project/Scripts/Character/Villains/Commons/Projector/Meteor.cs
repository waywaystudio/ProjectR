using Common;
using Common.Traps;
using UnityEngine;

namespace Character.Villains.Commons.Projector
{
    public class Meteor : TrapComponent, IProjectionProvider
    {
        public Vector3 SizeVector => new (radius, radius, 360f);
        public float CastingWeight => ProlongTime;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(Section.Execute,"MeteorExecution", () =>
                           {
                               if (TryGetTakerInSphere(out var takerList))
                               {
                                   takerList.ForEach(executor.ToTaker);
                               }
                           })
                           .Add(Section.Complete,"Execute", Execution);
        }

        public override void Execution() => SequenceInvoker.Execute();
    }
}
