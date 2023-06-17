using Common;
using Common.Traps;

namespace Character.Venturers.Ranger.Trap
{
    public class ExplodeTrapSample : TrapComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            sequencer.CompleteAction.Add("Execute", Execution);
            
            ExecuteAction.Add("MeteorExecution", () =>
            {
                if (TryGetTakerInSphere(out var takerList))
                {
                    takerList.ForEach(executor.Execute);
                }
            });
        }
        
        public override void Execution() => ExecuteAction.Invoke();
    }
}
