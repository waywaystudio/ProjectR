using Common;
using Common.Traps;
using UnityEngine;

namespace Character.Villains.Commons.Projector
{
    public class Meteor : TrapComponent, IProjectionProvider
    {
        public Vector3 SizeVector => new (radius * 2f, radius * 2f, radius * 2f);
        public float CastingWeight => ProlongTime;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Execute,"MeteorExecution", () =>
                           {
                               if (TryGetTakerInSphere(out var takerList))
                               {
                                   takerList.ForEach(executor.ToTaker);
                               }
                           })
                           .Add(SectionType.Complete,"Execute", Execution);
        }

        public override void Execution() => SequenceInvoker.Execute();
    }
}
