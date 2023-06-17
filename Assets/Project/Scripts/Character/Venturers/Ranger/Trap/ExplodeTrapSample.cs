using Common;
using Common.Traps;

namespace Character.Venturers.Ranger.Trap
{
    public class ExplodeTrapSample : TrapComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);


            SequenceBuilder.AddExecution("MeteorExecution", () =>
                           {
                               if (TryGetTakerInSphere(out var takerList)) 
                                   takerList.ForEach(executor.Execute);
                           })
                           .AddComplete("Execute", Execution);
        }
        
        public override void Execution() => ExecuteAction.Invoke();
    }
}
