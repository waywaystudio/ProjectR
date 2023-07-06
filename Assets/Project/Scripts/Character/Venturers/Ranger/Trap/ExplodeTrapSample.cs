using Common;
using Common.Traps;

namespace Character.Venturers.Ranger.Trap
{
    public class ExplodeTrapSample : TrapComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(Section.Execute,"MeteorExecution", () =>
                           {
                               if (TryGetTakerInSphere(out var takerList)) 
                                   takerList.ForEach(executor.ToTaker);
                           })
                           .Add(Section.Complete,"Execute", Execution);
        }
        
        public override void Execution() => SequenceInvoker.Execute();
    }
}
