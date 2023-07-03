using Common;
using Common.Traps;

namespace Character.Venturers.Ranger.Trap
{
    public class ExplodeTrapSample : TrapComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Execute,"MeteorExecution", () =>
                           {
                               if (TryGetTakerInSphere(out var takerList)) 
                                   takerList.ForEach(executor.ToTaker);
                           })
                           .Add(SectionType.Complete,"Execute", Execution);
        }
        
        public override void Execution() => SequenceInvoker.Execute();
    }
}
